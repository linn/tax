namespace Linn.Tax.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;

    public class VatReturnCalculationService : IVatReturnCalculationService
    {
        private readonly IQueryRepository<LedgerEntry> ledgerEntryRepository;

        private readonly IQueryRepository<LedgerMaster> ledgerMasterRepository;

        private readonly IQueryRepository<Purchase> purchaseLedger;

        private readonly IQueryRepository<PurchaseLedgerTransactionType> purchaseLedgerTransactionTypeRepository;

        private readonly IQueryRepository<Supplier> supplierRepository;

        public VatReturnCalculationService(
            IQueryRepository<LedgerEntry> ledgerEntryRepository,
            IQueryRepository<LedgerMaster> ledgerMasterRepository,
            IQueryRepository<Purchase> purchaseLedger,
            IQueryRepository<PurchaseLedgerTransactionType> purchaseLedgerTransactionTypeRepository,
            IQueryRepository<Supplier> supplierRepository)
        {
            this.ledgerEntryRepository = ledgerEntryRepository;
            this.ledgerMasterRepository = ledgerMasterRepository;
            this.purchaseLedger = purchaseLedger;
            this.purchaseLedgerTransactionTypeRepository = purchaseLedgerTransactionTypeRepository;
            this.supplierRepository = supplierRepository;
        }

        public VatReturn CalculateVatReturn()
        {
            var m = this.ledgerMasterRepository.FindAll().ToList().First();
            var periodsInCurrentQuarter = new List<int> { m.CurrentPeriod, m.CurrentPeriod - 1, m.CurrentPeriod - 2 };

            var salesTotals = this.ledgerEntryRepository
                .FilterBy(e => periodsInCurrentQuarter.Contains(e.LedgerPeriod)) 
                .GroupBy(l => l.TransactionType)
                .Select(g => new { TransactionType = g.Key, Total = g.Sum(x => x.BaseNetAmount) });

            var vatOnSalesTotals = this.ledgerEntryRepository
                .FilterBy(e => periodsInCurrentQuarter.Contains(e.LedgerPeriod))
                .GroupBy(l => l.TransactionType)
                .Select(g => new { TransactionType = g.Key, Total = g.Sum(x => x.BaseVatAmount) });

            var netGoodsSales = 
                salesTotals.ToList().First(t => t.TransactionType == "INV").Total // no other transaction types?
                - salesTotals.ToList().First(t => t.TransactionType == "CRED").Total;

            var netVatOnGoodsSales = 
                vatOnSalesTotals.ToList().First(t => t.TransactionType == "INV").Total
                - vatOnSalesTotals.ToList().First(t => t.TransactionType == "CRED").Total;

            var join2 = this.purchaseLedger
                .FilterBy(p => periodsInCurrentQuarter.Contains(p.LedgerPeriod))
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
                                     net = p.Sum(e => this.GetPaymentValue(e.tt, e.join1.pl.NetTotal)),
                                     vat = p.Sum(e => this.GetPaymentValue(e.tt, e.join1.pl.VatTotal))
                });

            // todo - find/calculate these numbers
            var canteenGoods = new decimal();
            var canteenVat = new decimal();
            decimal cashbookVat = new decimal();
            decimal intraDispatches = new decimal();
            decimal intraArrivals = new decimal();
            decimal intraVat = new decimal();

            decimal vatDueSales = (decimal)netVatOnGoodsSales + canteenVat;
            var enumerable = totals.ToList();
            decimal vatReclaimed = enumerable.First().vat + cashbookVat + intraVat;
            decimal totalVatDue = vatDueSales + intraVat;

            return new VatReturn
                       {
                           VatDueSales = vatDueSales,
                           VatDueAcquisitions = intraVat,
                           TotalVatDue = totalVatDue,
                           VatReclaimedCurrPeriod = vatReclaimed,
                           NetVatDue = totalVatDue - vatReclaimed,
                           TotalValueSalesExVat = netGoodsSales + canteenGoods,
                           TotalValuePurchasesExVat = enumerable.First().net,
                           TotalValueGoodsSuppliedExVat = intraDispatches,
                           TotalAcquisitionsExVat = intraArrivals
                       };
        }

        private decimal GetPaymentValue(PurchaseLedgerTransactionType transactionType, decimal absoluteValue)
        {
            return transactionType.DebitOrCredit == "C" ? absoluteValue : absoluteValue * -1;
        }
    }
}
