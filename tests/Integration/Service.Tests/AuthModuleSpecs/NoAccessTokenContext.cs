﻿namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using System.Collections.Generic;

    using Linn.Tax.Resources;
    using Linn.Tax.Service.Modules;

    using Nancy;
    using Nancy.Session;

    using NSubstitute;

    using NUnit.Framework;

    public class NoAccessTokenContext : ContextBase
    {
        [SetUp]
        public new void EstablishContext()
        {
            this.ApiService.ExchangeCodeForAccessToken(Arg.Any<string>()).Returns(new TokenResource { access_token = "blah" });
            this.Cfg = with =>
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
                                            return null;
                                        });
                                pipelines.BeforeRequest = b;
                            });
                    with.ApplicationStartup((container, pipelines) => { });
                };

            base.EstablishContext();
        }
    }
}
