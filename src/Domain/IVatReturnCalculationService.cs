namespace Linn.Tax.Domain
{
    using System.Collections.Generic;

    public interface IVatReturnCalculationService
    {
        VatReturn CalculateVatReturn(
            decimal salesGoodsTotal,
            decimal salesVatTotal,
            decimal canteenGoodsTotal,
            decimal canteenVatTotal,
            decimal purchasesGoodsTotal,
            decimal purchasesVatTotal,
            decimal cashbookAndOtherTotal,
            decimal instrastatDispatchesGoodsTotal,
            decimal intrastatArrivalsGoodsTotal,
            decimal intrastatArrivalsVatTotal);

        decimal GetSalesGoodsTotal();

        IDictionary<string, decimal> GetCanteenTotals();

        IDictionary<string, decimal> GetPurchasesTotals();

        IDictionary<string, decimal> GetIntrastatArrivals();

        decimal GetSalesVatTotal();

        decimal GetOtherJournals();
    }
}
