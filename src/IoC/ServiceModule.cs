namespace Linn.Tax.IoC
{
    using Amazon.SQS;
    using Autofac;

    using Linn.Common.Configuration;
    using Linn.Common.Logging;
    using Linn.Common.Logging.AmazonSqs;
    using Linn.Common.Proxy;
    using Linn.Tax.Domain;
    using Linn.Tax.Proxy;

    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // rest client proxies
            builder.RegisterType<RestClient>().As<IRestClient>();
            builder.RegisterType<HmrcApiProxy>().As<IHmrcApiService>().WithParameter("rootUri", ConfigurationManager.Configuration["HMRC_API_ROOT"]);
        }
    }
}