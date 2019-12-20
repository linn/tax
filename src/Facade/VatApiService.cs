namespace Linn.Tax.Facade
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Linn.Common.Facade;
    using Linn.Common.Proxy;
    using Linn.Common.Serialization.Json;
    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;

    public class VatApiService : IVatApiService
    {
        private readonly IHmrcApiService apiService;

        public VatApiService(IHmrcApiService apiService)
        {
            this.apiService = apiService;
        }

        public IResult<VatReturnResponseResource> SubmitVatReturn(VatReturnRequestResource resource, TokenResource token, string deviceId)
        {
            var apiResponse = this.apiService.SubmitVatReturn(resource, token, deviceId);
            var json = new JsonSerializer();

            if (apiResponse.StatusCode == HttpStatusCode.Created)
            {
                return new CreatedResult<VatReturnResponseResource>(json.Deserialize<VatReturnResponseResource>(apiResponse.Value));
            }

            var error = json.Deserialize<ErrorResponseResource>(apiResponse.Value);

            var message = $"{error.Message}.";

            if (error.Errors != null)
            {
                message = error.Errors.Aggregate(message, (current, e) => current + $" {e.Message}.");
            }
            
            return new BadRequestResult<VatReturnResponseResource>(message);
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