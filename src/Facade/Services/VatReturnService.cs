namespace Linn.Tax.Facade.Services
{
    using System;
    using System.Linq;
    using System.Net;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy;
    using Linn.Common.Serialization.Json;
    using Linn.Tax.Domain;
    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;

    public class VatReturnService : IVatReturnService
    {
        private readonly IHmrcApiService apiService;

        private readonly IVatReturnCalculationService calculationService;

        private readonly IRepository<VatReturnReceipt, int> vatReturnReceiptRepository;

        public VatReturnService(
            IHmrcApiService apiService, 
            IVatReturnCalculationService calculationService,
            IRepository<VatReturnReceipt, int> vatReturnReceiptRepository)
        {
            this.apiService = apiService;
            this.calculationService = calculationService;
            this.vatReturnReceiptRepository = vatReturnReceiptRepository;
        }

        public IResult<VatReturn> CalculateVatReturn(CalculationValuesResource resource)
        {
            return new SuccessResult<VatReturn>(this.calculationService.CalculateVatReturn(
                    resource.SalesGoodsTotal,
                    resource.SalesVatTotal,
                    resource.CanteenGoodsTotal,
                    resource.CanteenVatTotal,
                    resource.PurchasesGoodsTotal,
                    resource.PurchasesVatTotal,
                    resource.CashbookAndOtherTotal,
                    resource.PvaTotal));
        }

        public IResult<CalculationValuesResource> GetCalculationValues()
        {
            return new SuccessResult<CalculationValuesResource>(
                new CalculationValuesResource
                    {
                        SalesGoodsTotal = this.calculationService.GetSalesGoodsTotal(),
                        SalesVatTotal = this.calculationService.GetSalesVatTotal(),
                        CanteenGoodsTotal = this.calculationService.GetCanteenTotals()["goods"],
                        CanteenVatTotal = this.calculationService.GetCanteenTotals()["vat"],
                        LedgerEntries = this.calculationService.GetOtherJournals().Select(x => new NominalLedgerEntryResource
                            {
                                Amount = x.Amount,
                                Tref = x.Tref,
                                Narrative = x.Narrative,
                                Comments = x.Comments,
                                Description = x.Description,
                                CreditOrDebit = x.CreditOrDebit,
                                DatePosted = x.DatePosted.ToString("o")
                            }),
                        PurchasesGoodsTotal = this.calculationService.GetPurchasesTotals()["goods"],
                        PurchasesVatTotal = this.calculationService.GetPurchasesTotals()["vat"],
                        PvaTotal = this.calculationService.GetPvaTotal(),
                        IntrastatArrivalsGoodsTotal = 0m, // 0 post brexit
                        IntrastatArrivalsVatTotal = 0m, // 0 post brexit
                        InstrastatDispatchesGoodsTotal = 0m
                    });
        }

        public IResult<VatReturnReceiptResource> SubmitVatReturn(
            VatReturnSubmissionResource resource, 
            TokenResource token)
        {
            var apiResponse = this.apiService.SubmitVatReturn(resource, token);
            var json = new JsonSerializer();

            if (apiResponse.StatusCode == HttpStatusCode.Created)
            {
                var receipt = json.Deserialize<VatReturnReceiptResource>(apiResponse.Value);
                this.vatReturnReceiptRepository.Add(new VatReturnReceipt
                {
                    ProcessingDate = DateTime.Parse(receipt.ProcessingDate),
                    ChargeRefNumber = receipt.ChargeRefNumber,
                    FormBundleNumber = receipt.FormBundleNumber,
                    PaymentIndicator = receipt.PaymentIndicator
                });

                return new CreatedResult<VatReturnReceiptResource>(receipt);
            }

            var error = json.Deserialize<ErrorResponseResource>(apiResponse.Value);

            var message = $"{error.Message}.";

            if (error.Errors != null)
            {
                message = error.Errors.Aggregate(message, (current, e) => current + $" {e.Message}.");
            }
            
            return new BadRequestResult<VatReturnReceiptResource>(message);
        }

        public IResult<ObligationsResource> GetObligations(
            ObligationsRequestResource resource, 
            TokenResource token)
        {
            var apiResponse = this.apiService.GetVatObligations(resource, token);
            var json = new JsonSerializer();
            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                return new SuccessResult<ObligationsResource>(json
                    .Deserialize<ObligationsResource>(apiResponse.Value));
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
