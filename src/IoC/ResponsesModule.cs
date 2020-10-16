namespace Linn.Tax.IoC
{
    using Autofac;

    using Linn.Common.Facade;
    using Linn.Tax.Domain;
    using Linn.Tax.Facade.ResourceBuilders;

    public class ResponsesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // resource builders
            builder.RegisterType<VatReturnResourceBuilder>().As<IResourceBuilder<VatReturn>>();
        }
    }
}