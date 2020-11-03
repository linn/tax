namespace Linn.Tax.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Linn.Common.Persistence;

    public class VatReturnCalculationService : IVatReturnCalculationService
    {
        private readonly IQueryRepository<SalesLedgerEntry> ledgerEntryRepository;

        private readonly IQueryRepository<Purchase> purchaseLedger;

        private readonly IQueryRepository<PurchaseLedgerTransactionType> purchaseLedgerTransactionTypeRepository;

        private readonly IQueryRepository<Supplier> supplierRepository;

        private readonly IQueryRepository<NominalLedgerEntry> nominalLedgerRepository;

        private readonly IDatabaseService databaseService;

        private readonly List<int> periodsInCurrentQuarter;

        public VatReturnCalculationService(
            IQueryRepository<SalesLedgerEntry> ledgerEntryRepository,
            IQueryRepository<LedgerMaster> ledgerMasterRepository,
            IQueryRepository<Purchase> purchaseLedger,
            IQueryRepository<PurchaseLedgerTransactionType> purchaseLedgerTransactionTypeRepository,
            IQueryRepository<Supplier> supplierRepository,
            IQueryRepository<NominalLedgerEntry> nominalLedgerRepository,
            IDatabaseService databaseService)
        {
            this.ledgerEntryRepository = ledgerEntryRepository;
            this.purchaseLedger = purchaseLedger;
            this.purchaseLedgerTransactionTypeRepository = purchaseLedgerTransactionTypeRepository;
            this.supplierRepository = supplierRepository;
            this.nominalLedgerRepository = nominalLedgerRepository;
            var m = ledgerMasterRepository.FindAll().ToList().First();
            this.periodsInCurrentQuarter = new List<int> { 1438, 1439, 1440 }; // { m.CurrentPeriod, m.CurrentPeriod - 1, m.CurrentPeriod - 2 };
            this.databaseService = databaseService;
        }

        public VatReturn CalculateVatReturn()
        {
            var salesTotals = this.ledgerEntryRepository
                .FilterBy(e => this.periodsInCurrentQuarter.Contains(e.LedgerPeriod)) 
                .GroupBy(l => l.TransactionType)
                .Select(g => new { TransactionType = g.Key, Total = g.Sum(x => x.BaseNetAmount) });

            var vatOnSalesTotals = this.ledgerEntryRepository
                .FilterBy(e => this.periodsInCurrentQuarter.Contains(e.LedgerPeriod))
                .GroupBy(l => l.TransactionType)
                .Select(g => new { TransactionType = g.Key, Total = g.Sum(x => x.BaseVatAmount) });

            var netGoodsSales = 
                salesTotals.ToList().First(t => t.TransactionType == "INV").Total // no other transaction types?
                - salesTotals.ToList().First(t => t.TransactionType == "CRED").Total;

            var netVatOnGoodsSales = 
                vatOnSalesTotals.ToList().First(t => t.TransactionType == "INV").Total
                - vatOnSalesTotals.ToList().First(t => t.TransactionType == "CRED").Total;

            // todo - find these values
            decimal cashbookVat = new decimal(0);
            decimal intraDispatches = new decimal(0);
            var intraArrivals = this.GetIntrastatArrivals();

            decimal vatDueSales = (decimal)netVatOnGoodsSales + this.GetCanteenTotals()["vat"];
            decimal vatReclaimed = this.GetPurchasesTotals()["vat"] + cashbookVat + intraArrivals["vat"];
            decimal totalVatDue = vatDueSales + intraArrivals["vat"];

            return new VatReturn
                       {
                           VatDueSales = vatDueSales,
                           VatDueAcquisitions = intraArrivals["vat"],
                           TotalVatDue = totalVatDue,
                           VatReclaimedCurrPeriod = vatReclaimed,
                           NetVatDue = totalVatDue - vatReclaimed,
                           TotalValueSalesExVat = this.GetCanteenTotals()["goods"] + netGoodsSales,
                           TotalValuePurchasesExVat = this.GetPurchasesTotals()["net"],
                           TotalValueGoodsSuppliedExVat = intraDispatches,
                           TotalAcquisitionsExVat = intraArrivals["net"]
                       };
        }

        private static decimal GetPaymentValue(PurchaseLedgerTransactionType transactionType, decimal absoluteValue)
        {
            return transactionType.DebitOrCredit == "C" ? absoluteValue : absoluteValue * -1;
        }

        private Dictionary<string, decimal> GetPurchasesTotals()
        {
            var join2 = this.purchaseLedger
                .FilterBy(p => this.periodsInCurrentQuarter.Contains(p.LedgerPeriod))
                .Join(
                    this.supplierRepository.FindAll(),
                    pl => new { pl.SupplierId },
                    s => new { s.SupplierId },
                    (pl, s) => new { pl, s })
                .Where(j => j.s.LiveOnOracle == "Y")
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
                           { "net", totals.Net },
                           { "vat", totals.Vat }
                       };
        }

        private Dictionary<string, decimal> GetCanteenTotals()
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
                        ({this.periodsInCurrentQuarter[0]}, {this.periodsInCurrentQuarter[1]}, {this.periodsInCurrentQuarter[2]})
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
            var net = decimal.Parse(res.ToString());
            var goods = net / 1.2m; 
            var vat = 0.2m * goods;
            return new Dictionary<string, decimal>
                       {
                           { "goods", Math.Round(goods, 2) },
                           { "vat", Math.Round(vat, 2) }
                       };
        }

        private Dictionary<string, decimal> GetIntrastatArrivals()
        {
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
                        and trunc(imp.date_Created) between TO_DATE('01-oct-2019') and TO_DATE('31-dec-2019')
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
            var net = decimal.Parse(res.ToString());
            var vat = 0.2m * net;
            return new Dictionary<string, decimal>
                       {
                           { "net", Math.Round(net, 2) },
                           { "vat", Math.Round(vat, 2) }
                       };
        }
    }
}
