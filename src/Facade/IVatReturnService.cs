namespace Linn.Tax.Facade
{
    using Linn.Common.Facade;
    using Linn.Tax.Domain;
    using Linn.Tax.Resources;

    public interface IVatReturnService
    {
        IResult<VatReturn> CalculateVatReturn();

        IResult<VatReturnResponseResource> SubmitVatReturn(VatReturnRequestResource resource, TokenResource token, string deviceId);

        IResult<ObligationsResource> GetObligations(ObligationsRequestResource resource, TokenResource token, string deviceId);
    }
}