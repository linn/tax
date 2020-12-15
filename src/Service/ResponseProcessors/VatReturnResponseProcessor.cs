namespace Linn.Tax.Service.ResponseProcessors
{
    using Linn.Common.Facade;
    using Linn.Common.Nancy.Facade;
    using Linn.Tax.Domain;

    public class VatReturnResponseProcessor : JsonResponseProcessor<VatReturn>
    {
        public VatReturnResponseProcessor(IResourceBuilder<VatReturn> resourceBuilder)
            : base(resourceBuilder, "vat-return", 1)
        {
        }
    }
}
