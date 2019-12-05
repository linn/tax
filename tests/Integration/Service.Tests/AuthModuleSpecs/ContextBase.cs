namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using System;
    using Linn.Tax.Proxy;
    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase : NancyContextBase
    {
       
        protected string RedirectLocation;

        protected IHmrcApiService ApiService;

        protected Action<ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator> cfg;

        [SetUp]
        public void EstablishContext()
        {
            this.ApiService = Substitute.For<IHmrcApiService>();
            var bootstrapper = new ConfigurableBootstrapper(this.cfg);
            this.Browser = new Browser(bootstrapper);
        }
    }
}