namespace Linn.Tax.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Linn.Common.Persistence;

    public class VatReturnCalculationService : IVatReturnCalculationService
    {
        private readonly IQueryRepository<SalesLedgerEntry> ledgerEntryRepository;

        private readonly IQueryRepository<Purchase> purchaseLedger;

        private readonly IQueryRepository<PurchaseLedgerTransactionType> 
            purchaseLedgerTransactionTypeRepository;

        private readonly IQueryRepository<Supplier> supplierRepository;

        private readonly IDatabaseService databaseService;

        private readonly List<int> periodsInLastQuarter;

        private readonly IQueryRepository<ImportBook> importBooksRepository;

        private readonly IQueryRepository<NominalLedgerEntry> nominalLedgerRepository;

        public VatReturnCalculationService(
            IQueryRepository<SalesLedgerEntry> ledgerEntryRepository,
            IQueryRepository<Purchase> purchaseLedger,
            IQueryRepository<PurchaseLedgerTransactionType> purchaseLedgerTransactionTypeRepository,
            IQueryRepository<Supplier> supplierRepository,
            IQueryRepository<LedgerMaster> ledgerMasterRepository,
            IQueryRepository<ImportBook> importBooksRepository,
            IQueryRepository<NominalLedgerEntry> nominalLedgerRepository,
            IDatabaseService databaseService)
        {
            this.ledgerEntryRepository = ledgerEntryRepository;
            this.nominalLedgerRepository = nominalLedgerRepository;
            this.purchaseLedger = purchaseLedger;
            this.purchaseLedgerTransactionTypeRepository 
                = purchaseLedgerTransactionTypeRepository;
            this.supplierRepository = supplierRepository;
            var m = ledgerMasterRepository.FindAll().ToList().FirstOrDefault();
            this.periodsInLastQuarter = new List<int>
                                               {
                                                   m.CurrentPeriod - 1,
                                                   m.CurrentPeriod - 2, 
                                                   m.CurrentPeriod - 3
                                               };
            this.importBooksRepository = importBooksRepository;
            this.databaseService = databaseService;
        }

        public decimal GetSalesGoodsTotal()
        {
            var salesTotals = this.ledgerEntryRepository
                .FilterBy(e => this.periodsInLastQuarter.Contains(e.LedgerPeriod))
                .GroupBy(l => l.TransactionType)
                .Select(g => new { TransactionType = g.Key, Total = g.Sum(x => x.BaseNetAmount) });

            return
                salesTotals.ToList().First(t => t.TransactionType == "INV").Total // no other transaction types?
                - salesTotals.ToList().First(t => t.TransactionType == "CRED").Total;
        }

        public IEnumerable<NominalLedgerEntry> GetCanteenCredits()
        {
            return this.nominalLedgerRepository.FilterBy(
                e => this.periodsInLastQuarter.Contains(e.PeriodNumber) && e.NominalAccountId == 4039
                                                                        && e.CreditOrDebit == "C");
        }

        public decimal GetSalesVatTotal()
        {
            var vatOnSalesTotals = this.ledgerEntryRepository
                .FilterBy(e => this.periodsInLastQuarter.Contains(e.LedgerPeriod))
                .GroupBy(l => l.TransactionType)
                .Select(g => new { TransactionType = g.Key, Total = g.Sum(x => x.BaseVatAmount) });
            
            return vatOnSalesTotals.ToList().First(t => t.TransactionType == "INV").Total
                - vatOnSalesTotals.ToList().First(t => t.TransactionType == "CRED").Total;
        }

        public decimal GetPvaTotal()
        {
            return this.importBooksRepository
                .FilterBy(i => i.Pva == "Y" && this.periodsInLastQuarter.Contains(i.PeriodNumber))
                .Sum(x => x.LinnVat);
        }

        public IDictionary<string, decimal> GetCanteenTotals()
        {
            var sql = $@"select sum(ct.total_currency) tot
                        from 
                        cashbook_transactions ct,
                        cashbook_transaction_lines ctl,
                        ledger_periods lp,
                        nominal_accounts  n,
                        cashbooks c,
                        linn_nominals ln,
                        linn_departments ld,
                        cashbook_alloc_codes cac
                        where
                        ct.cashbook_id = ctl.cashbook_id 
                        and ct.payment_or_lodgement = 'L'
                        and ct.period_number in 
                        ({this.periodsInLastQuarter[0]}, 
                        {this.periodsInLastQuarter[1]}, 
                        {this.periodsInLastQuarter[2]})
                        and ct.cashbook_id = c.cashbook_id
                        and ctl.alloc_code = 'CE'
                        and ct.payment_or_lodgement = ctl.payment_or_lodgement
                        and ct.tref = ctl.tref 
                        and ct.period_number = lp.period_number 
                        and ctl.nomacc_id = n.nomacc_id (+)
                        and ctl.cashbook_id = cac.cashbook_id
                        and ctl.payment_or_lodgement = cac.payment_or_lodgement
                        and ctl.alloc_code = cac.alloc_code 
                        and n.nominal = ln.nominal_code (+)
                        and n.department = ld.department_code (+)";

            var res = this.databaseService.ExecuteQuery(sql).Tables[0].Rows[0][0];
            var total = res == DBNull.Value ? 0 : decimal.Parse(res.ToString());
           
            var goods = total / 1.2m;
           
            var vat = 0.2m * goods;
            return new Dictionary<string, decimal>
                       {
                           { "goods", Math.Round(goods, 2) },
                           { "vat", Math.Round(vat, 2) }
                       };
        }

        public VatReturn CalculateVatReturn(
            decimal salesGoodsTotal,
            decimal salesVatTotal,
            decimal canteenGoodsTotal,
            decimal canteenVatTotal,
            decimal purchasesGoodsTotal,
            decimal purchasesVatTotal,
            decimal cashbookAndOtherTotal,
            decimal pvaTotal) 
        {
            decimal vatReclaimed = purchasesVatTotal + cashbookAndOtherTotal + pvaTotal;
            decimal totalVatDue = salesVatTotal + canteenVatTotal + pvaTotal;
            return new VatReturn
                       {
                           VatDueSales = totalVatDue,
                           VatDueAcquisitions = 0m,
                           TotalVatDue = totalVatDue,
                           VatReclaimedCurrPeriod = vatReclaimed,
                           NetVatDue = vatReclaimed - totalVatDue,
                           TotalValueSalesExVat = Math.Round(canteenGoodsTotal + salesGoodsTotal, 0),
                           TotalValuePurchasesExVat = Math.Round(purchasesGoodsTotal, 0),
                           TotalValueGoodsSuppliedExVat = 0m,
                           TotalAcquisitionsExVat = 0m
                       };
        }

        public IDictionary<string, decimal> GetPurchasesTotals()
        {
            var join2 = this.purchaseLedger
                .FilterBy(p => this.periodsInLastQuarter.Contains(p.LedgerPeriod))
                .Join(
                    this.supplierRepository.FindAll(),
                    pl => new { pl.SupplierId },
                    s => new { s.SupplierId },
                    (pl, s) => new { pl, s })
                .Join(
                    this.purchaseLedgerTransactionTypeRepository.FindAll(),
                    join1 => new { join1.pl.TransactionType },
                    tt => new { tt.TransactionType },
                    (join1, tt) => new { join1, tt }).Where(r => r.tt.TransactionCategory == "INV")
                .ToList();

            var totals = join2
                .GroupBy(purchase => purchase.tt.TransactionCategory)
                .Select(p => new
                                 {
                                     Net = p.Sum(e => GetPaymentValue(e.tt, e.join1.pl.NetTotal)),
                                     Vat = p.Sum(e => GetPaymentValue(e.tt, e.join1.pl.VatTotal))
                                 })
                .First();
            return new Dictionary<string, decimal>
                       {
                           { "goods", totals.Net },
                           { "vat", totals.Vat }
                       };
        }

        public IEnumerable<NominalLedgerEntry> GetOtherJournals()
        {
            return this.nominalLedgerRepository.FilterBy(
                e => this.periodsInLastQuarter.Contains(e.PeriodNumber) && e.NominalAccountId == 1012);
        }

        private static decimal GetPaymentValue(
            PurchaseLedgerTransactionType transactionType, 
            decimal absoluteValue)
        {
            return transactionType.DebitOrCredit == "C" 
                       ? absoluteValue : absoluteValue * -1;
        }
    }
}
