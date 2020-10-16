namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using System;

    using Linn.Common.Facade;
    using Linn.Tax.Proxy;
    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase : NancyContextBase
    {
        protected IHmrcApiService ApiService { get; set; }

        protected Action<ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator> Cfg { get; set; }

        protected string RedirectLocation { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.ApiService = Substitute.For<IHmrcApiService>();
            var bootstrapper = new ConfigurableBootstrapper(this.Cfg);
            this.Browser = new Browser(bootstrapper);
        }
    }
}