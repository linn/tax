namespace Linn.Tax.Service.Modules
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using Linn.Tax.Domain;
    using Linn.Common.Configuration;
    using Linn.Tax.Resources;
    using Linn.Tax.Service.Models;

    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Responses;

    public sealed class AuthenticationModule : NancyModule
    {
        private IHmrcApiService apiService;

        public AuthenticationModule(IHmrcApiService apiService)
        {
            this.apiService = apiService;
            this.Get("/auth", _ => this.AuthRedirect());
            this.Get("/success", _ => this.GetSuccess());
        }

        private object SilentRenew()
        {
            return this.Negotiate.WithView("silent-renew");
        }

        private object AuthRedirect()
        {
            var root = ConfigurationManager.Configuration["HMRC_API_ROOT"];
            var clientId = ConfigurationManager.Configuration["CLIENT_ID"];
            var redirect = "http://localhost:61798/success";
            var location = $"{root}/oauth/authorize?response_type=code&client_id={clientId}&redirect_uri={redirect}&scope=hello";
            return new RedirectResponse(location);
        }

        private object GetSuccess()
        {
            
            // test open access api call
            var helloWorld = this.apiService.HelloWorld();

           // test application restricted api call
           var helloApp = this.apiService.HelloApplication();

            // test user restricted (OAuth 2.0) api call
            var resource = this.Bind<AuthenticationResource>();

            var code = resource.Code;

            var token = this.apiService.ExchangeCodeForAccessToken(code);

            var helloUser = this.apiService.HelloUser(token);

            return helloUser;
        }
    }
}