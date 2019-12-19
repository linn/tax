namespace Linn.Tax.Service.Modules
{
    using Linn.Common.Facade;
    using Linn.Tax.Facade;
    using Linn.Tax.Resources;
    using Linn.Tax.Service.Models;

    using Nancy;
    using Nancy.ModelBinding;

    public sealed class VatReturnModule : NancyModule
    {
        private readonly IVatApiService vatApiService;

        public VatReturnModule(IVatApiService vatApiService)
        {
            this.vatApiService = vatApiService;
            this.Post("/tax/return", _ => this.SubmitTaxReturn());
        }

        private object SubmitTaxReturn()
        {
            var resource = this.Bind<VatReturnRequestResource>();
            
            var result = this.vatApiService.SubmitVatReturn(resource, (TokenResource)this.Session["access_token"], this.Request.Cookies["device_id"]);
            if (result is CreatedResult<VatReturnResponseResource> createdResult)
            {
                return this.Negotiate
                    .WithModel(createdResult.Data)
                    .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                    .WithView("Index");
            }

            return this.Negotiate
                .WithModel((BadRequestResult<VatReturnResponseResource>)result)
                .WithStatusCode(400)
                .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                .WithView("Index");
        }
    }
}