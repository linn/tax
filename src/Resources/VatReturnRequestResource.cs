namespace Linn.Tax.Resources
{
    public class VatReturnRequestResource
    {
        public string periodKey { get; set; }

        public decimal vatDueSales { get; set; }

        public decimal vatDueAcquisitions { get; set; }

        public decimal totalVatDue { get; set; }

        public decimal vatReclaimedCurrPeriod { get; set; }

        public decimal netVatDue { get; set; }

        public decimal totalValueSalesExVAT { get; set; }

        public decimal totalValuePurchasesExVAT { get; set; }

        public decimal totalValueGoodsSuppliedExVAT { get; set; }

        public decimal totalAcquisitionsExVAT { get; set; }

        public bool finalised { get; set; }
    }
}