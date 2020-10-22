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

        private readonly List<int> periodsInCurrentQuarter;

        public VatReturnCalculationService(
            IQueryRepository<SalesLedgerEntry> ledgerEntryRepository,
            IQueryRepository<LedgerMaster> ledgerMasterRepository,
            IQueryRepository<Purchase> purchaseLedger,
            IQueryRepository<PurchaseLedgerTransactionType> purchaseLedgerTransactionTypeRepository,
            IQueryRepository<Supplier> supplierRepository,
            IQueryRepository<NominalLedgerEntry> nominalLedgerRepository)
        {
            this.ledgerEntryRepository = ledgerEntryRepository;
            this.purchaseLedger = purchaseLedger;
            this.purchaseLedgerTransactionTypeRepository = purchaseLedgerTransactionTypeRepository;
            this.supplierRepository = supplierRepository;
            this.nominalLedgerRepository = nominalLedgerRepository;
            var m = ledgerMasterRepository.FindAll().ToList().First();
            this.periodsInCurrentQuarter = new List<int> { m.CurrentPeriod, m.CurrentPeriod - 1, m.CurrentPeriod - 2 };
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
            decimal intraArrivals = new decimal(0);
            decimal intraVat = new decimal(0);

            decimal vatDueSales = (decimal)netVatOnGoodsSales + this.GetCanteenTotals()["vat"];
            decimal vatReclaimed = this.GetPurchasesTotals()["vat"] + cashbookVat + intraVat;
            decimal totalVatDue = vatDueSales + intraVat;

            return new VatReturn
                       {
                           VatDueSales = vatDueSales,
                           VatDueAcquisitions = intraVat,
                           TotalVatDue = totalVatDue,
                           VatReclaimedCurrPeriod = vatReclaimed,
                           NetVatDue = totalVatDue - vatReclaimed,
                           TotalValueSalesExVat = this.GetCanteenTotals()["goods"] + netGoodsSales,
                           TotalValuePurchasesExVat = this.GetPurchasesTotals()["net"],
                           TotalValueGoodsSuppliedExVat = intraDispatches,
                           TotalAcquisitionsExVat = intraArrivals
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
            var vatOrCanteenExpr = 
                new Func<IGrouping<int, NominalLedgerEntry>, bool>(
                    t => this.nominalLedgerRepository
                             .FilterBy(e => e.JournalNumber == t.Key).Any(x => x.NominalAccountId == 4039)
                         && this.nominalLedgerRepository
                             .FilterBy(e => e.JournalNumber == t.Key).Any(x => x.NominalAccountId == 1012));

            var canteenJournalNumbers = this.nominalLedgerRepository
                .FilterBy(n => this.periodsInCurrentQuarter.Contains(n.PeriodNumber)
                               && n.TransactionType == "JRNL")
                .GroupBy(n => n.JournalNumber).AsEnumerable()
                .Where(vatOrCanteenExpr).Select(g => g.Key);

            var canteenNominalLedgerEntries = this.nominalLedgerRepository
                .FilterBy(l => canteenJournalNumbers.Contains(l.JournalNumber) && l.CreditOrDebit == "C").ToList();

            var vat = canteenNominalLedgerEntries.Sum(x => x.Amount);
            var goods = canteenNominalLedgerEntries.Sum(x => decimal // todo - find these values in a less terrible way
                .Parse(
                    Regex.Match(
                            x.Comments.Replace(",", string.Empty),
                            @"[0-9]+(\.[0-9]+)?")
                        .Value));
            return new Dictionary<string, decimal>
                       {
                           { "goods", goods },
                           { "vat", vat }
                       };
        }
    }
}
