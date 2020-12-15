namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Tax.Resources;

    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCallingBackWithDeviceId : NoAccessTokenContext
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
                        with.Cookie(new Dictionary<string, string> { { "device_id", "a_guid" } });
                    }).Result;
        }

        [Test]
        public void ShouldPreserveDeviceIdAsACookie()
        {
            Assert.That(this.Response.Cookies.Any(c => c.Value == "a_guid"));
        }
    }
}
