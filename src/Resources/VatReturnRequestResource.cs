namespace Linn.Tax.Resources
{
    using System;
    using System.Collections.Generic;

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

        public bool DoNotTrack { get; set; }

        public int WindowWidth { get; set; }

        public int WindowHeight { get; set; }

        public List<string> BrowserPlugins { get; set; }
        
        public string UserAgentString { get; set; }

        public string Username { get; set; }

        public List<string> LocalIps { get; set; }

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        public int ScalingFactor { get; set; }

        public int TimezoneOffset { get; set; }
    }
}