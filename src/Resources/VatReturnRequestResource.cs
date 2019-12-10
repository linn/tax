namespace Linn.Tax.Resources
{
    public class VatReturnRequestResource
    {
        public int Vrn { get; set; }

        public string PeriodKey { get; set; }

        public decimal VatDueSales { get; set; }

        public decimal VatDueAcquisitions { get; set; }

        public decimal TotalVatDue { get; set; }

        public decimal VatReclaimedCurrPeriod { get; set; }

        public decimal NetVatDue { get; set; }

        public decimal TotalValueSalesExVat { get; set; }

        public decimal TotalValuePurchasesExVat { get; set; }

        public decimal TotalValueGoodsSuppliedExVat { get; set; }

        public decimal TotalAcquisitionsExVat { get; set; }

        public bool Finalised { get; set; }
    }
}