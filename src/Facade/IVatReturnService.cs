namespace Linn.Tax.Facade
{
    using Linn.Common.Facade;
    using Linn.Tax.Resources;

    public interface IVatReturnService
    {
        IResult<VatReturnResponseResource> SubmitVatReturn(VatReturnRequestResource resource, TokenResource token);
    }
}