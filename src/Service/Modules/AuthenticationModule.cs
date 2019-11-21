namespace Linn.Tax.Service.Modules
{
    using Linn.Common.Configuration;
    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;

    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Responses;

    public sealed class AuthenticationModule : NancyModule
    {
        private readonly IHmrcApiService apiService;

        public AuthenticationModule(IHmrcApiService apiService)
        {
            this.apiService = apiService;
            this.Get("/auth", _ => this.AuthRedirect());
            this.Get("/tax/signin-oidc-silent", _ => this.SilentRenew());
            this.Get("/tax/redirect", _ => this.AuthReturn());
            this.Get("/tax/return", _ => this.SubmitTaxReturn());
        }

        private object SilentRenew()
        {
            return this.Negotiate.WithView("silent-renew");
        }

        private object AuthRedirect()
        {
            var root = ConfigurationManager.Configuration["HMRC_API_ROOT"];
            var clientId = ConfigurationManager.Configuration["CLIENT_ID"];
            var redirect = "http://localhost:61798/tax/redirect";
            var location = $"{root}/oauth/authorize?response_type=code&client_id={clientId}&redirect_uri={redirect}&scope=write:vat";
            return new RedirectResponse(location);
        }

        private object AuthReturn()
        {
            var resource = this.Bind<AuthenticationResource>();

            var code = resource.Code;

            ConfigurationManager.Configuration["access_token"] = this.apiService.ExchangeCodeForAccessToken(code);

            return "success";
        }

        private object SubmitTaxReturn()
        {
            var formData = new VatReturnRequestResource
                               {
                                   periodKey = "A001",
                                   vatDueSales = new decimal(105.50),
                                   vatDueAcquisitions = new decimal(-100.45),
                                   totalVatDue = new decimal(5.05),
                                   vatReclaimedCurrPeriod= new decimal(105.15),
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