namespace Linn.Tax.Facade
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Tax.Resources;

    public interface IVatApiService
    {
        IResult<VatReturnResponseResource> SubmitVatReturn(VatReturnRequestResource resource, TokenResource token, string deviceId);

        IResult<IEnumerable<ObligationResource>> GetObligations(ObligationsRequestResource resource, TokenResource token, string deviceId);
    }
}