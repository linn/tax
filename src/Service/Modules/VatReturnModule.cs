namespace Linn.Tax.Service.Modules
{
    using Linn.Common.Configuration;
    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;

    using Nancy;

    public sealed class VatReturnModule : NancyModule
    {
        private IHmrcApiService apiService;

        public VatReturnModule(IHmrcApiService apiService)
        {
            this.apiService = apiService;
            this.Get("/tax/return", _ => this.SubmitTaxReturn());
        }

        private object SubmitTaxReturn()
        {
            var formData = new VatReturnRequestResource
            {
                periodKey = "A001",
                vatDueSales = new decimal(105.50),
                vatDueAcquisitions = new decimal(-100.45),
                totalVatDue = new decimal(5.05),
                vatReclaimedCurrPeriod = new decimal(105.15),
                netVatDue = new decimal(100.10),
                totalValueSalesExVAT = 300,
                totalValuePurchasesExVAT = 300,
                totalValueGoodsSuppliedExVAT = 3000,
                totalAcquisitionsExVAT = 3000,
                finalised = true
            };

            var helloUser = this.apiService.SubmitVatReturn(ConfigurationManager.Configuration["access_token"], formData);

            return helloUser;
        }
    }
}