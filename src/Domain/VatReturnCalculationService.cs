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

        private readonly IQueryRepository<LedgerMaster> ledgerMasterRepository;

        private readonly IQueryRepository<LedgerPeriod> ledgerPeriodRepository;

        public VatReturnCalculationService(
            IQueryRepository<SalesLedgerEntry> ledgerEntryRepository,
            IQueryRepository<Purchase> purchaseLedger,
            IQueryRepository<PurchaseLedgerTransactionType> purchaseLedgerTransactionTypeRepository,
            IQueryRepository<Supplier> supplierRepository,
            IQueryRepository<LedgerMaster> ledgerMasterRepository,
            IQueryRepository<LedgerPeriod> ledgerPeriodRepository,
            IDatabaseService databaseService)
        {
            this.ledgerEntryRepository = ledgerEntryRepository;
            this.ledgerPeriodRepository = ledgerPeriodRepository;
            this.purchaseLedger = purchaseLedger;
            this.purchaseLedgerTransactionTypeRepository 
                = purchaseLedgerTransactionTypeRepository;
            this.supplierRepository = supplierRepository;
            this.ledgerMasterRepository = ledgerMasterRepository;
            var m = this.ledgerMasterRepository.FindAll().ToList().FirstOrDefault();
            this.periodsInLastQuarter = new List<int>
                                               {
                                                   m.CurrentPeriod - 1,
                                                   m.CurrentPeriod - 2, 
                                                   m.CurrentPeriod - 3
                                               };
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

        public decimal GetSalesVatTotal()
        {
            var vatOnSalesTotals = this.ledgerEntryRepository
                .FilterBy(e => this.periodsInLastQuarter.Contains(e.LedgerPeriod))
                .GroupBy(l => l.TransactionType)
                .Select(g => new { TransactionType = g.Key, Total = g.Sum(x => x.BaseVatAmount) });
            
            return vatOnSalesTotals.ToList().First(t => t.TransactionType == "INV").Total
                - vatOnSalesTotals.ToList().First(t => t.TransactionType == "CRED").Total;
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
            decimal instrastatDispatchesGoodsTotal, 
            decimal intrastatArrivalsGoodsTotal, 
            decimal intrastatArrivalsVatTotal) 
        {
            decimal vatReclaimed = purchasesVatTotal + cashbookAndOtherTotal + intrastatArrivalsVatTotal;
            decimal totalVatDue = salesVatTotal + canteenVatTotal + intrastatArrivalsVatTotal;
            return new VatReturn
                       {
                           VatDueSales = salesVatTotal + canteenVatTotal,
                           VatDueAcquisitions = intrastatArrivalsVatTotal,
                           TotalVatDue = totalVatDue,
                           VatReclaimedCurrPeriod = vatReclaimed,
                           NetVatDue = vatReclaimed - totalVatDue,
                           TotalValueSalesExVat = canteenGoodsTotal + salesGoodsTotal,
                           TotalValuePurchasesExVat = purchasesGoodsTotal,
                           TotalValueGoodsSuppliedExVat = instrastatDispatchesGoodsTotal,
                           TotalAcquisitionsExVat = intrastatArrivalsGoodsTotal
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
            var sql = $@"select *
                         from nominal_ledger
                         where nomacc_id = 1012 
                         AND period_number in 
                         ({this.periodsInLastQuarter[0]}, 
                         {this.periodsInLastQuarter[1]}, 
                         {this.periodsInLastQuarter[2]})";

            var result = this.databaseService.ExecuteQuery(sql);
            
            return (from DataRow row in result.Tables[0].Rows
                    select row.ItemArray
                    into values
                    select new NominalLedgerEntry 
                               {
                                   Tref = Convert.ToInt32(values[0]),
                                   Amount = values[7].ToString().Equals("D") ? 
                                                Convert.ToDecimal(values[5]) 
                                                : 0m - Convert.ToDecimal(values[5]),
                        Comments = values[11].ToString(),
                                   CreditOrDebit = values[7].ToString(),
                                   DatePosted = Convert.ToDateTime(values[3]),
                                   Description = values[10].ToString(),
                                   JournalNumber = Convert.ToInt32(values[1]),
                                   Narrative = values[9].ToString(),
                                   TransactionType = values[6].ToString()
                               }).ToList();
        }

        public IDictionary<string, decimal> GetIntrastatArrivals()
        {
            var dateString = this.GetDateStringFromPeriods();
            var sql = $@"select sum(order_vat) from
                        (select imp.impbook_id,
                        imp.supplier_id,
                        supp.supplier_name,
                        sum(iod.vat_value) order_vat
                        from impbooks imp,
                        suppliers supp,
                        impbook_order_details iod,
                        countries co
                        where imp.impbook_id=iod.impbook_id
                        and imp.supplier_id=supp.supplier_id
                        and trunc(imp.date_Created) between {dateString}
                        and imp.date_Cancelled is null
                        and co.country_code=supp.country
                        and co.eec_member='Y'
                        and imp.date_cancelled is null
                        group by imp.impbook_id,
                        imp.supplier_id,
                        supp.supplier_name,
                        imp.linn_vat,
                        imp.total_import_value)";

            var res = this.databaseService.ExecuteQuery(sql).Tables[0].Rows[0][0];
            var vat = decimal.Parse(res.ToString());
            var goods = 5m * vat;
            return new Dictionary<string, decimal>
                       {
                           { "vat", Math.Round(vat, 2) },
                           { "goods", Math.Round(goods, 2) }
                       };
        }

        private static decimal GetPaymentValue(
            PurchaseLedgerTransactionType transactionType, 
            decimal absoluteValue)
        {
            return transactionType.DebitOrCredit == "C" 
                       ? absoluteValue : absoluteValue * -1;
        }

        private string GetDateStringFromPeriods()
        {
            var periods = this.ledgerPeriodRepository
                .FilterBy(p => this.periodsInLastQuarter.Contains(p.PeriodNumber))
                .OrderBy(p => p.PeriodNumber).ToList();

            var start = periods.First();
            var end = periods.Last();

            var startMonth = start.MonthName.Substring(0, 3);
            var startYear = start.MonthName.Substring(3, 4);
            var endMonth = end.MonthName.Substring(0, 3);
            var endYear = end.MonthName.Substring(3, 4);
            return $@"TO_DATE('01-{startMonth}-{startYear}') and TO_DATE('31-{endMonth}-{endYear}')";
        }
    }
}
