namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using System.Linq;

    using FluentAssertions;

    using Linn.Tax.Resources;

    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCallingBackWithNoDeviceId : NoAccessTokenContext
    {
        [SetUp]
        public void SetUp()
        {
            this.ApiService.ExchangeCodeForAccessToken(Arg.Any<string>()).Returns(new TokenResource
                                                                                      {
                                                                                          access_token = "token"
                                                                                      });

            this.Response = this.Browser.Get(
                "/tax/redirect",
                with =>
                    {
                    }).Result;
        }

        [Test]
        public void ShouldBeAssignedADeviceIdAsACookie()
        {
            Assert.That(this.Response.Cookies.Any(c => c.EncodedName == "device_id"));
        }

        [Test]
        public void ShouldCallApi()
        {
            this.ApiService.Received().ExchangeCodeForAccessToken(Arg.Any<string>());
        }

        [Test]
        public void ShouldStoreAccessToken()
        {
            var token = this.Response.Context.Request.Session["access_token"];
            ((TokenResource)token).access_token.Should().Be("token");
        }

        [Test]
        public void ShouldRedirect()
        {
            this.Response.ShouldHaveRedirectedTo("/tax/view-obligations");
        }
    }
}
