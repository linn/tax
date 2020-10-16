namespace Linn.Tax.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;

    public class VatReturnCalculationService : IVatReturnCalculationService
    {
        private readonly IQueryRepository<LedgerEntry> ledgerEntryRepository;

        private readonly IQueryRepository<LedgerMaster> ledgerMasterRepository;

        private readonly IQueryRepository<SalesLedgerTransactionType> transactionTypeRepository;

        private readonly IQueryRepository<Purchase> purchaseLedger;

        private readonly IQueryRepository<PurchaseLedgerTransactionType> purchaseLedgerTransactionTypeRepository;

        private readonly IQueryRepository<Supplier> supplierRepository;

        public VatReturnCalculationService(
            IQueryRepository<LedgerEntry> ledgerEntryRepository,
            IQueryRepository<LedgerMaster> ledgerMasterRepository,
            IQueryRepository<SalesLedgerTransactionType> transactionTypeRepository,
            IQueryRepository<Purchase> purchaseLedger,
            IQueryRepository<PurchaseLedgerTransactionType> purchaseLedgerTransactionTypeRepository,
            IQueryRepository<Supplier> supplierRepository)
        {
            this.ledgerEntryRepository = ledgerEntryRepository;
            this.ledgerMasterRepository = ledgerMasterRepository;
            this.transactionTypeRepository = transactionTypeRepository;
            this.purchaseLedger = purchaseLedger;
            this.purchaseLedgerTransactionTypeRepository = purchaseLedgerTransactionTypeRepository;
            this.supplierRepository = supplierRepository;
        }

        public VatReturn CalculateVatReturn()
        {
            var m = this.ledgerMasterRepository.FindAll().ToList().FirstOrDefault();
            var periodsInCurrentQuarter = new List<int> { 1438, 1439, 1440 }; //m.CurrentPeriod, m.CurrentPeriod - 1, m.CurrentPeriod - 2 };
            var salesTotals = this.ledgerEntryRepository.FilterBy(e => 
                periodsInCurrentQuarter.Contains(e.LedgerPeriod)) 
                .GroupBy(l => l.TransactionType)
                .Select(g => new { TransactionType = g.Key, Total = g.Sum(x => x.BaseNetAmount) });

            var vatOnSalesTotals = this.ledgerEntryRepository.FilterBy(e =>
                    periodsInCurrentQuarter.Contains(e.LedgerPeriod))
                .GroupBy(l => l.TransactionType)
                .Select(g => new { TransactionType = g.Key, Total = g.Sum(x => x.BaseVatAmount) });

            var netHGoodsSales = salesTotals.ToList().FirstOrDefault(t => t.TransactionType == "INV")?.Total // TODO - other transaction types?
                      - salesTotals.ToList().FirstOrDefault(t => t.TransactionType == "CRED")?.Total; // TODO - did you use repository?
            var netVatOnGoodsSales = vatOnSalesTotals.ToList().FirstOrDefault(t => t.TransactionType == "INV")?.Total
                                - vatOnSalesTotals.ToList().FirstOrDefault(t => t.TransactionType == "CRED")?.Total;
            // CANTEEN

            // DISPATCHES

            // ARRIVALS

            // to get PURCHASES goods/vat values
            var join2 = this.purchaseLedger.FilterBy(p => periodsInCurrentQuarter.Contains(p.LedgerPeriod))
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

            var totals = join2.GroupBy(purchase => purchase.tt.TransactionCategory)
                .Select(p => new
                                 {
                                     net = p.Sum(e => this.GetPaymentValue(e.tt, e.join1.pl.NetTotal)),
                                     vat = p.Sum(e => this.GetPaymentValue(e.tt, e.join1.pl.VatTotal))
                });

            return new VatReturn();
        }

        private decimal GetPaymentValue(PurchaseLedgerTransactionType transactionType, decimal absoluteValue)
        {
            return transactionType.DebitOrCredit == "C" ? absoluteValue : absoluteValue * -1;
        }
    }
}