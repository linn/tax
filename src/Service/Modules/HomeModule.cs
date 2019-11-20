namespace Linn.Production.Service.Modules
{
    using Linn.Tax.Domain;
    using Linn.Tax.Service.Models;

    using Nancy;
    using Nancy.Responses;

    public sealed class HomeModule : NancyModule
    {
        private IHmrcApiService apiService;

        public HomeModule(IHmrcApiService apiService)
        {
            this.apiService = apiService;

            this.Get("/", args => new RedirectResponse("/tax"));
            this.Get("/tax", _ => this.GetApp());

            this.Get(@"^(.*)$", _ => this.GetApp());
        }

        private object SilentRenew()
        {
            return this.Negotiate.WithView("silent-renew");
        }

        private object GetApp()
        {
            return this.Negotiate.WithModel(ApplicationSettings.Get()).WithView("Index");
        }
    }
}