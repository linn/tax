namespace Linn.Tax.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Tax.Domain;
    using Linn.Tax.Resources;

    public interface IVatReturnService
    {
        IResult<CalculationValuesResource> GetCalculationValues();

        IResult<VatReturn> CalculateVatReturn(CalculationValuesResource resource);

        IResult<VatReturnReceiptResource> SubmitVatReturn(VatReturnSubmissionResource resource, TokenResource token);

        IResult<ObligationsResource> GetObligations(ObligationsRequestResource resource, TokenResource token);
    }
}
