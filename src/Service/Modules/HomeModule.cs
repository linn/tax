namespace Linn.Tax.Service.Modules
{
    using Linn.Tax.Service.Models;

    using Nancy;

    public sealed class HomeModule : NancyModule
    {
        public HomeModule()
        {
            this.Get("/", args => this.GetApp());
            this.Get("/tax/signin-oidc-silent", _ => this.SilentRenew());
            this.Get("/tax/signin-oidc-client", _ => this.GetApp());
            this.Get("/tax/signin-oidc-silent", _ => this.SilentRenew());

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