namespace Linn.Tax.Facade.ResourceBuilders
{
    using Linn.Common.Configuration;
    using Linn.Common.Facade;
    using Linn.Tax.Domain;
    using Linn.Tax.Resources;

    public class VatReturnResourceBuilder : IResourceBuilder<VatReturn>
    {
        public VatReturnResource Build(VatReturn model)
        {
            return new VatReturnResource
                       {
                           // Vrn = int.Parse(ConfigurationManager.Configuration["VRN"]),
                           VatDueSales = model.VatDueSales,
                           VatDueAcquisitions = model.VatDueAcquisitions,
                           TotalVatDue = model.TotalVatDue,
                           VatReclaimedCurrPeriod = model.VatReclaimedCurrPeriod,
                           NetVatDue = model.NetVatDue,
                           TotalValueSalesExVat = model.TotalValueSalesExVat,
                           TotalValuePurchasesExVat = model.TotalValuePurchasesExVat,
                           TotalValueGoodsSuppliedExVat = model.TotalValueGoodsSuppliedExVat,
                           TotalAcquisitionsExVat = model.TotalAcquisitionsExVat,
                       };
        }

        public string GetLocation(VatReturn model)
        {
            throw new System.NotImplementedException();
        }

        object IResourceBuilder<VatReturn>.Build(VatReturn vatReturn) => this.Build(vatReturn);
    }
}
