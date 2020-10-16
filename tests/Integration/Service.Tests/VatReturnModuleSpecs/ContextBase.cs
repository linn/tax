namespace Linn.Tax.Service.Tests.VatReturnModuleSpecs
{
    using Linn.Tax.Facade.Services;
    using Linn.Tax.Proxy;
    using Linn.Tax.Service.Modules;

    using Nancy.Session;
    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase : NancyContextBase
    {
        protected IHmrcApiService ApiService { get; set; }

        protected IVatReturnService VatReturnService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.ApiService = Substitute.For<IHmrcApiService>();
            this.VatReturnService = Substitute.For<IVatReturnService>();
            var bootstrapper = new ConfigurableBootstrapper(
                with =>
                    {
                        with.Dependency(this.ApiService);
                        with.Dependency(this.VatReturnService);
                        with.Module<VatReturnModule>();
                        with.ApplicationStartup((container, pipelines) => { CookieBasedSessions.Enable(pipelines); });
                    });
            
            this.Browser = new Browser(bootstrapper);
        }
    }
}
