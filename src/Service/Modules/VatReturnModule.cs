namespace Linn.Tax.Service.Modules
{
    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;
    using Linn.Tax.Service.Models;

    using Nancy;
    using Nancy.ModelBinding;

    public sealed class VatReturnModule : NancyModule
    {
        private readonly IHmrcApiService apiService;

        public VatReturnModule(IHmrcApiService apiService)
        {
            this.apiService = apiService;
            this.Post("/tax/return", _ => this.SubmitTaxReturn());
        }

        private object SubmitTaxReturn()
        {
            var resource = this.Bind<VatReturnRequestResource>();

            var result = this.apiService.SubmitVatReturn(resource);

            return this.Negotiate
                .WithModel(result)
                .WithMediaRangeModel("text/html", ApplicationSettings.Get)
                .WithView("Index");
        }
    }
}