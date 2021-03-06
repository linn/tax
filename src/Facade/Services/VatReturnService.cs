﻿namespace Linn.Tax.Facade.Services
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
                    resource.InstrastatDispatchesGoodsTotal,
                    resource.IntrastatArrivalsGoodsTotal,
                    resource.IntrastatArrivalsVatTotal));
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
                        CashbookAndOtherTotal = this.calculationService.GetOtherJournals(),
                        PurchasesGoodsTotal = this.calculationService.GetPurchasesTotals()["goods"],
                        PurchasesVatTotal = this.calculationService.GetPurchasesTotals()["vat"],
                        IntrastatArrivalsGoodsTotal = this.calculationService.GetIntrastatArrivals()["goods"],
                        IntrastatArrivalsVatTotal = this.calculationService.GetIntrastatArrivals()["vat"],
                        InstrastatDispatchesGoodsTotal = 0 // didn't manage to obtain this value, still needs to be entered for now
                });
        }

        public IResult<VatReturnReceiptResource> SubmitVatReturn(
            VatReturnSubmissionResource resource, 
            TokenResource token, 
            string deviceId)
        {
            var apiResponse = this.apiService.SubmitVatReturn(resource, token, deviceId);
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
            TokenResource token, 
            string deviceId)
        {
            var apiResponse = this.apiService.GetVatObligations(resource, token, deviceId);
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
