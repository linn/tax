namespace Linn.Tax.Service.Modules
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Tax.Facade;
    using Linn.Tax.Resources;
    using Linn.Tax.Service.Models;

    using Nancy;
    using Nancy.ModelBinding;

    public sealed class ObligationsModule : NancyModule
    {
        private readonly IVatApiService vatApiService;

        public ObligationsModule(IVatApiService vatApiService)
        {
            this.vatApiService = vatApiService;
            this.Post("/tax/obligations", _ => this.GetObligations());
        }

        private object GetObligations()
        {
            var resource = this.Bind<ObligationsRequestResource>();

            var result = this.vatApiService.GetObligations(resource, (TokenResource)this.Session["access_token"], this.Request.Cookies["device_id"]);
            if (result is SuccessResult<ObligationsResource> successResult)
            {
                return this.Negotiate
                    .WithModel(successResult.Data)
                    .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                    .WithView("Index");
            }

            return this.Negotiate
                .WithModel((BadRequestResult<IEnumerable<ObligationsResource>>)result)
                .WithStatusCode(400)
                .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                .WithView("Index");
        }
    }
}