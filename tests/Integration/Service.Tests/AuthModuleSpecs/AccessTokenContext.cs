namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using System.Collections.Generic;

    using Linn.Tax.Resources;
    using Linn.Tax.Service.Modules;

    using Nancy;
    using Nancy.Session;

    using NUnit.Framework;

    public class AccessTokenContext : ContextBase
    {
        [SetUp]
        public new void EstablishContext()
        {
            this.cfg = with =>
                {
                    with.Dependency(this.ApiService);
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
                };
            base.EstablishContext();
        }
    }
}