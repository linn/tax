namespace Linn.Tax.Service.Tests.VatReturnModuleSpecs
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Tax.Resources;

    using Nancy;
    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPostingVatReturn : ContextBase
    {
        private VatReturnResource resource;
        [SetUp]
        public void SetUp()
        {
            this.resource = new VatReturnResource();

            this.VatReturnService
                .SubmitVatReturn(Arg.Any<VatReturnResource>(), Arg.Any<TokenResource>(), Arg.Any<string>())
                .Returns(new CreatedResult<VatReturnReceiptResource>(new VatReturnReceiptResource
                                                                          {
                                                                              ChargeRefNumber = "ref",
                                                                              FormBundleNumber = "bundle"
                                                                          }));

            this.Response = this.Browser.Post(
                "tax/return",
                with =>
                    {
                        with.Cookie(new Dictionary<string, string> { { "device_id", "a_guid" } });
                        with.JsonBody(this.resource);
                        with.Accept("application/vnd.hmrc.1.0+json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallService()
        {
            this.VatReturnService.Received().SubmitVatReturn(
                Arg.Any<VatReturnResource>(), 
                Arg.Any<TokenResource>(), 
                Arg.Any<string>());
        }

        [Test]
        public void ShouldReturnResource()
        {
            var res = this.Response.Body.DeserializeJson<VatReturnReceiptResource>();
            res.ChargeRefNumber.Should().Be("ref");
        }
    }
}
