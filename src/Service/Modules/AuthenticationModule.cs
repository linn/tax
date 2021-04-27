namespace Linn.Tax.Service.Modules
{
    using System;

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
            this.Get("tax/auth", _ => this.AuthRedirect());
            this.Get("/tax/redirect", _ => this.Callback());
            this.Post("/tax/test/fraud-prevention-headers", _ => this.TestFraudPreventionHeaders());
        }

        private object AuthRedirect()
        {
            if (((TokenResource)this.Session["access_token"])?.access_token != null)
            {
                var token = (TokenResource)this.Session["access_token"];

                try
                {
                    var newToken = this.apiService.RefreshToken(token.refresh_token);
                    this.Session["access_token"] = newToken;
                }
                catch (AccessTokenExpiredException e)
                {
                    this.Session["access_token"] = null;
                    return new RedirectResponse($"{ConfigurationManager.Configuration["APP_ROOT"]}/tax/auth");
                }

                return new RedirectResponse($"{ConfigurationManager.Configuration["APP_ROOT"]}/tax/view-obligations");
            }

            var root = ConfigurationManager.Configuration["HMRC_API_ROOT"];
            var clientId = ConfigurationManager.Configuration["CLIENT_ID"];
            var redirect = ConfigurationManager.Configuration["AUTH_CALLBACK_URI"];
            var location = $"{root}/oauth/authorize?response_type=code&client_id={clientId}&redirect_uri={redirect}&scope=write:vat+read:vat";
            return new RedirectResponse(location);
        }

        private object Callback()
        {
            if (!this.Request.Cookies.ContainsKey("device_id") || this.Request.Cookies["device_id"] == null)
            {
                this.Request.Cookies["device_id"] = Guid.NewGuid().ToString();
            }

            var resource = this.Bind<AuthenticationResource>();

            var code = resource.Code;
            this.Session["access_token"] = this.apiService.ExchangeCodeForAccessToken(code);

            return new RedirectResponse($"{ConfigurationManager.Configuration["APP_ROOT"]}/tax/view-obligations")
                .WithCookie("device_id", this.Request.Cookies["device_id"], DateTime.MaxValue);
        }

        private object TestFraudPreventionHeaders()
        {
            var resource = this.Bind<FraudPreventionMetadataResource>();

            var result = this.apiService.TestFraudPreventionHeaders(resource, this.Request.Cookies["device_id"]);

            return result.Value;
        }
    }
}
