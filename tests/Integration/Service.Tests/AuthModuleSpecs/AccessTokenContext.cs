namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Tax.Domain;
    using Linn.Tax.Facade.ResourceBuilders;
    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;
    using Linn.Tax.Service.Modules;
    using Linn.Tax.Service.ResponseProcessors;

    using Nancy;
    using Nancy.Session;
    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;

    public class AccessTokenContext : NancyContextBase
    {
        protected IHmrcApiService ApiService { get; set; }

        protected string RedirectLocation { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.ApiService = Substitute.For<IHmrcApiService>();
            var bootstrapper = new ConfigurableBootstrapper(with =>
                {
                    with.Dependency(this.ApiService);
                    with.Dependency<IResourceBuilder<VatReturn>>(new VatReturnResourceBuilder());
                    with.ResponseProcessor<VatReturnResponseProcessor>();
                    with.Module<AuthenticationModule>();
                    with.RequestStartup(
                        (container, pipelines, context) =>
                            {
                                var b = new BeforePipeline();
                                b.AddItemToEndOfPipeline(
                                    x =>
                                        {
                                            context.Request.Session = new Session(new Dictionary<string, object>());
                                            context.Request.Session["access_token"] =
                                                new TokenResource { access_token = "token", refresh_token = "refresh" };
                                            return null;
                                        });
                                pipelines.BeforeRequest = b;
                            });
                });
            this.Browser = new Browser(bootstrapper);
        }
    }
}
