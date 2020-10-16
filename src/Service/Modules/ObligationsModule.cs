﻿namespace Linn.Tax.Service.Modules
{
    using Linn.Common.Facade;
    using Linn.Tax.Facade.Services;
    using Linn.Tax.Resources;
    using Linn.Tax.Service.Models;

    using Nancy;
    using Nancy.ModelBinding;

    public sealed class ObligationsModule : NancyModule
    {
        private readonly IVatReturnService vatReturnService;

        public ObligationsModule(IVatReturnService vatReturnService)
        {
            this.vatReturnService = vatReturnService;
            this.Post("/tax/obligations", _ => this.GetObligations());
        }

        private object GetObligations()
        {
            var resource = this.Bind<ObligationsRequestResource>();

            var result = this.vatReturnService.GetObligations(resource, (TokenResource)this.Session["access_token"], this.Request.Cookies["device_id"]);
            if (result is SuccessResult<ObligationsResource> successResult)
            {
                return this.Negotiate
                    .WithModel(successResult.Data)
                    .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                    .WithView("Index");
            }

            return this.Negotiate
                .WithModel((BadRequestResult<ObligationsResource>)result)
                .WithStatusCode(400)
                .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                .WithView("Index");
        }
    }
}
