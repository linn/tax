namespace Linn.Tax.Facade.Services
{
    using System.Linq;
    using System.Net;

    using Linn.Common.Facade;
    using Linn.Common.Proxy;
    using Linn.Common.Serialization.Json;
    using Linn.Tax.Domain;
    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;

    public class VatReturnService : IVatReturnService
    {
        private readonly IHmrcApiService apiService;

        private readonly IVatReturnCalculationService calculationService;

        public VatReturnService(IHmrcApiService apiService, IVatReturnCalculationService calculationService)
        {
            this.apiService = apiService;
            this.calculationService = calculationService;
        }

        public IResult<VatReturn> CalculateVatReturn()
        {
            return new SuccessResult<VatReturn>(this.calculationService.CalculateVatReturn());
        }

        public IResult<VatReturnReceiptResource> SubmitVatReturn(VatReturnSubmissionResource resource, TokenResource token, string deviceId)
        {
            var apiResponse = this.apiService.SubmitVatReturn(resource, token, deviceId);
            var json = new JsonSerializer();

            if (apiResponse.StatusCode == HttpStatusCode.Created)
            {
                return new CreatedResult<VatReturnReceiptResource>(json.Deserialize<VatReturnReceiptResource>(apiResponse.Value));
            }

            var error = json.Deserialize<ErrorResponseResource>(apiResponse.Value);

            var message = $"{error.Message}.";

            if (error.Errors != null)
            {
                message = error.Errors.Aggregate(message, (current, e) => current + $" {e.Message}.");
            }
            
            return new BadRequestResult<VatReturnReceiptResource>(message);
        }

        public IResult<ObligationsResource> GetObligations(ObligationsRequestResource resource, TokenResource token, string deviceId)
        {
            var apiResponse = this.apiService.GetVatObligations(resource, token, deviceId);
            var json = new JsonSerializer();
            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                return new SuccessResult<ObligationsResource>(json.Deserialize<ObligationsResource>(apiResponse.Value));
            }

            return BuildErrorResponse(apiResponse);
        }

        private static dynamic BuildErrorResponse(IRestResponse<string> response)
        {
            var json = new JsonSerializer();
            var error = json.Deserialize<ErrorResponseResource>(response.Value);

            var message = $"{error.Message}.";

            if (error.Errors != null)
            {
                message = error.Errors.Aggregate(message, (current, e) => current + $" {e.Message}.");
            }

            return new BadRequestResult<ObligationsResource>(message);
        }
    }
}
