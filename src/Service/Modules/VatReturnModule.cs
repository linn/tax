namespace Linn.Tax.Service.Modules
{
    using Linn.Common.Facade;
    using Linn.Tax.Facade.Services;
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
            this.Get("/tax/return", _ => this.GetTaxReturnCalculationResult());
            this.Get("/tax/return/calculation-values", _ => this.GetCalculationValues());
        }

        private object GetTaxReturnCalculationResult()
        {
            var resource = this.Bind<CalculationValuesResource>();

            var result = this.vatReturnService.CalculateVatReturn(resource);
            return this.Negotiate
                .WithModel(result)
                .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                .WithView("Index");
        }

        private object GetCalculationValues()
        {
            var result = this.vatReturnService.GetCalculationValues();
            if (result is SuccessResult<CalculationValuesResource> successResult)
            {
                return this.Negotiate
                    .WithModel(successResult.Data)
                    .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                    .WithView("Index");
            }

            return this.Negotiate
                .WithModel((BadRequestResult<CalculationValuesResource>)result)
                .WithStatusCode(400)
                .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                .WithView("Index");
        }

        private object SubmitTaxReturn()
        {
            var resource = this.Bind<VatReturnSubmissionResource>();
            
            var result = this.vatReturnService.SubmitVatReturn(resource, (TokenResource)this.Session["access_token"]);
            if (result is CreatedResult<VatReturnReceiptResource> createdResult)
            {
                return this.Negotiate
                    .WithModel(createdResult.Data)
                    .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                    .WithView("Index");
            }

            return this.Negotiate
                .WithModel((BadRequestResult<VatReturnReceiptResource>)result)
                .WithStatusCode(400)
                .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                .WithView("Index");
        }
    }
}
