namespace Linn.Tax.IoC
{
    using System.Collections.Generic;

    using Autofac;
    using Autofac.Core;

    using Linn.Common.Configuration;
    using Linn.Common.Facade;
    using Linn.Common.Proxy;
    using Linn.Tax.Domain;
    using Linn.Tax.Facade.Services;
    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;

    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // domain services
            builder.RegisterType<VatReturnCalculationService>().As<IVatReturnCalculationService>();

            // oracle proxy
            builder.RegisterType<DatabaseService>().As<IDatabaseService>();

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
