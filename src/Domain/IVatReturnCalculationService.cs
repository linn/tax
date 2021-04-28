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
            decimal pvaTotal);

        decimal GetSalesGoodsTotal();

        IDictionary<string, decimal> GetCanteenTotals();

        IDictionary<string, decimal> GetPurchasesTotals();

        IEnumerable<NominalLedgerEntry> GetOtherJournals();

        decimal GetSalesVatTotal();

        decimal GetPvaTotal();
    }
}
