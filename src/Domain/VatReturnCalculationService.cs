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

        public VatReturnCalculationService(
            IQueryRepository<LedgerEntry> ledgerEntryRepository,
            IQueryRepository<LedgerMaster> ledgerMasterRepository,
            IQueryRepository<SalesLedgerTransactionType> transactionTypeRepository)
        {
            this.ledgerEntryRepository = ledgerEntryRepository;
            this.ledgerMasterRepository = ledgerMasterRepository;
            this.transactionTypeRepository = transactionTypeRepository;
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
            //select
            //sum(pl_pack.get_payment_value(pl.pl_trans_type, pl.base_net_total)) net,
            //sum(pl_pack.get_payment_value(pl.pl_trans_type, pl.base_vat_total)) vat
            //    from purchase_ledger pl, suppliers s,
            //pl_trans_types tt
            //where pl.ledger_period  in (1438, 1439, 1440)
            //and trans_category = 'INV'
            //and pl.pl_Trans_type = tt.pl_trans_type
            //and pl.supplier_id = s.supplier_id
            //and s.live_on_oracle = 'Y'
            //group by tt.trans_category;

            return new VatReturn();
        }

        private decimal GetPaymentValue(PurchaseLedgerTransactionType transactionType, decimal absoluteValue)
        {
            return transactionType.DebitOrCredit == "C" ? absoluteValue : absoluteValue * -1;
        }
    }
}