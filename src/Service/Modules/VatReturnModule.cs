﻿namespace Linn.Tax.Service.Modules
{
    using Linn.Common.Facade;
    using Linn.Tax.Facade;
    using Linn.Tax.Resources;
    using Linn.Tax.Service.Models;

    using Nancy;
    using Nancy.ModelBinding;

    public sealed class VatReturnModule : NancyModule
    {
        private readonly IVatReturnService vatReturnService;

        public VatReturnModule(IVatReturnService vatReturnService)
        {
            this.vatReturnService = vatReturnService;
            this.Post("/tax/return", _ => this.SubmitTaxReturn());
        }

        private object SubmitTaxReturn()
        {
            if (!this.Request.Cookies.ContainsKey("device_id") || this.Request.Cookies["device_id"] == null)
            {
                this.Request.Cookies["device_id"] = System.Guid.NewGuid().ToString();
            }

            var resource = this.Bind<VatReturnRequestResource>();
            
            var result = this.vatReturnService.SubmitVatReturn(resource, (TokenResource)this.Session["access_token"]);
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