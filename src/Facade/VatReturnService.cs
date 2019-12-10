﻿namespace Linn.Tax.Facade
{
    using System.Net;

    using Linn.Common.Facade;
    using Linn.Common.Serialization.Json;
    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;

    public class VatReturnService : IVatReturnService
    {
        private readonly IHmrcApiService apiService;

        public VatReturnService(IHmrcApiService apiService)
        {
            this.apiService = apiService;
        }

        public IResult<VatReturnResponseResource> SubmitVatReturn(VatReturnRequestResource resource, TokenResource token)
        {
            var apiResponse = this.apiService.SubmitVatReturn(resource, token);
            var json = new JsonSerializer();

            if (apiResponse.StatusCode == HttpStatusCode.Created)
            {
                return new CreatedResult<VatReturnResponseResource>(json.Deserialize<VatReturnResponseResource>(apiResponse.Value));
            }

            var error = json.Deserialize<ErrorResponseResource>(apiResponse.Value);

            var message = $"{error.Message}.";

            if (error.Errors != null)
            {
                foreach (var e in error.Errors)
                {
                    message += $" {e.Message}.";
                }
            }
            
            return new BadRequestResult<VatReturnResponseResource>(message);
        }
    }
}