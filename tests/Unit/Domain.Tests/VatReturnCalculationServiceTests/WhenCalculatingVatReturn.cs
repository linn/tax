﻿namespace Linn.Tax.Domain.Tests.VatReturnCalculationServiceTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenCalculatingVatReturn : ContextBase
    {
        private VatReturn result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.CalculateVatReturn(
                3630687.06m,
                170757.20m,
                8739.33m,
                1747.87m,
                2031905.52m,
                338770.88m,
                -12257.17m,
                15950.10m);
        }

        [Test]
        public void ShouldReturnVatReturnWithCorrectValues()
        {
            this.result.Should().BeOfType<VatReturn>();
            
            this.result.VatDueSales.Should().Be(188455.17m);
            this.result.VatDueAcquisitions.Should().Be(0m);
            this.result.TotalVatDue.Should().Be(188455.17m);
            this.result.VatReclaimedCurrPeriod.Should().Be(342463.81m);
            this.result.NetVatDue.Should().Be(154008.64m);
            this.result.TotalValueSalesExVat.Should().Be(3639426m);
            this.result.TotalValuePurchasesExVat.Should().Be(2031906m);
            this.result.TotalValueGoodsSuppliedExVat.Should().Be(0m);
            this.result.TotalAcquisitionsExVat.Should().Be(0m);
        }
    }
}
