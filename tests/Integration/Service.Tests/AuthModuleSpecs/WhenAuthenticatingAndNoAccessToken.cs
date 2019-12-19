namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using Linn.Common.Configuration;
    using Linn.Tax.Resources;
    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;
    using NUnit.Framework.Internal;

    public class WhenAuthenticatingAndNoAccessToken : NoAccessTokenContext
    {
        [SetUp]
        public void SetUp()
        {
            // When no access token is present, user should be redirected to HMRC website to authenticate.
            var root = ConfigurationManager.Configuration["HMRC_API_ROOT"];
            var clientId = ConfigurationManager.Configuration["CLIENT_ID"];
            var redirect = ConfigurationManager.Configuration["AUTH_CALLBACK_URI"];
            this.RedirectLocation = $"{root}/oauth/authorize?response_type=code&client_id={clientId}&redirect_uri={redirect}&scope=write:vat+read:vat";

                this.ApiService.RefreshToken(Arg.Any<string>()).Returns(new TokenResource());
                this.Response = this.Browser.Get(
                    "/tax/auth",
                    with =>
                        {
                            with.Header("Accept", "application/json");
                        }).Result;
        }

        [Test]
        public void ShouldRedirect()
        {
            this.Response.ShouldHaveRedirectedTo(this.RedirectLocation);
        }
    }
}