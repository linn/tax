namespace Linn.Tax.IoC
{
    using System.Collections.Generic;

    using Amazon.SQS;
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Activators.Reflection;

    using Linn.Common.Configuration;
    using Linn.Common.Logging;
    using Linn.Common.Logging.AmazonSqs;
    using Linn.Common.Proxy;
    using Linn.Tax.Facade;
    using Linn.Tax.Proxy;

    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // facade services
            builder.RegisterType<VatReturnService>().As<IVatReturnService>();

            // rest client proxies
            builder.RegisterType<RestClient>().As<IRestClient>();
            builder.RegisterType<HmrcApiProxy>()
                .As<IHmrcApiService>()
                .WithParameters(
                new List<Parameter>
                    {
                        new NamedParameter("rootUri", ConfigurationManager.Configuration["HMRC_API_ROOT"]),
                        new NamedParameter("clientId", ConfigurationManager.Configuration["CLIENT_ID"]),
                        new NamedParameter("clientSecret", ConfigurationManager.Configuration["CLIENT_SECRET"])
                    });
        }
    }
}