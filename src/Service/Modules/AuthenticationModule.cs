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
            this.Get("/auth", _ => this.AuthRedirect());
            this.Get("/tax/redirect", _ => this.Callback());
        }

        private object AuthRedirect()
        {
            // is this session already auth'd?
            if (this.Session["access_token"] != null && ((TokenResource)this.Session["access_token"]).access_token != null)
            {
                var token = (TokenResource)this.Session["access_token"];

                try
                {
                    this.Session["access_token"] = this.apiService.RefreshToken(token.refresh_token);
                }
                catch (AccessTokenExpiredException e)
                {
                    this.Session["access_token"] = null;
                    return new RedirectResponse($"{ConfigurationManager.Configuration["APP_ROOT"]}/auth");
                }

                return new RedirectResponse($"{ConfigurationManager.Configuration["APP_ROOT"]}/tax/submit-return");
            }

            var root = ConfigurationManager.Configuration["HMRC_API_ROOT"];
            var clientId = ConfigurationManager.Configuration["CLIENT_ID"];
            var redirect = ConfigurationManager.Configuration["AUTH_CALLBACK_URI"];
            var location = $"{root}/oauth/authorize?response_type=code&client_id={clientId}&redirect_uri={redirect}&scope=write:vat";
            return new RedirectResponse(location);
        }

        private object Callback()
        {
            var resource = this.Bind<AuthenticationResource>();

            var code = resource.Code;

            this.Session["access_token"] = this.apiService.ExchangeCodeForAccessToken(code);

            return new RedirectResponse($"{ConfigurationManager.Configuration["APP_ROOT"]}/tax/submit-return");
        }
    }
}