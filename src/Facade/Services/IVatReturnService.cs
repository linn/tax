namespace Linn.Tax.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Tax.Domain;
    using Linn.Tax.Resources;

    public interface IVatReturnService
    {
        IResult<VatReturn> CalculateVatReturn();

        IResult<CalculationValuesResource> GetCalculationValues();

        IResult<VatReturnReceiptResource> SubmitVatReturn(VatReturnSubmissionResource resource, TokenResource token, string deviceId);

        IResult<ObligationsResource> GetObligations(ObligationsRequestResource resource, TokenResource token, string deviceId);
    }
}
