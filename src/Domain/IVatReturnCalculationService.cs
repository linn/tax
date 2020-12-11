namespace Linn.Tax.Domain
{
    using System.Collections.Generic;

    public interface IVatReturnCalculationService
    {
        VatReturn CalculateVatReturn();

        decimal GetSalesGoodsTotal();

        IDictionary<string, decimal> GetCanteenTotals();

        IDictionary<string, decimal> GetPurchasesTotals();

        IDictionary<string, decimal> GetIntrastatArrivals();

        decimal GetSalesVatTotal();

        decimal GetOtherJournals();
    }
}
