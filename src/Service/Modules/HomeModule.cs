namespace Linn.Tax.Service.Modules
{
    using Linn.Tax.Proxy;
    using Linn.Tax.Service.Models;

    using Nancy;

    public sealed class HomeModule : NancyModule
    {
        private IHmrcApiService apiService;

        public HomeModule(IHmrcApiService apiService)
        {
            this.apiService = apiService;

            this.Get("/", args => this.GetApp());
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