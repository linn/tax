namespace Linn.Tax.Resources
{
    using System.Collections;
    using System.Collections.Generic;

    public class CalculationValuesResource
    {
        public decimal SalesGoodsTotal { get; set; }

        public decimal SalesVatTotal { get; set; }

        public decimal CanteenGoodsTotal { get; set; }

        public decimal CanteenVatTotal { get; set; }

        public decimal PurchasesGoodsTotal { get; set; }

        public decimal PurchasesVatTotal { get; set; }

        public decimal CashbookAndOtherTotal { get; set; }

        public decimal InstrastatDispatchesGoodsTotal { get; set; }

        public decimal IntrastatArrivalsGoodsTotal { get; set; }

        public decimal IntrastatArrivalsVatTotal { get; set; }

        public IEnumerable<NominalLedgerEntryResource> LedgerEntries { get; set; }
    }
}
