namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using FluentAssertions;

    using Linn.Tax.Resources;
    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;


    public class WhenAuthenticatingAndAccessToken : AccessTokenContext
    {
        [SetUp]
        public void SetUp()
        {
            this.ApiService.RefreshToken(Arg.Any<string>()).Returns(new TokenResource { access_token = "new token" });
            
            // the user should have their access token refreshed automatically and then be redirected
            this.RedirectLocation = "/tax/submit-return";

            this.Response = this.Browser.Get(
                "/tax/auth",
                with =>
                    {
                        with.Header("Accept", "application/json");
                    }).Result;
        }

        [Test]
        public void ShouldCallApi()
        {
            this.ApiService.Received().RefreshToken(Arg.Any<string>());
        }

        [Test]
        public void ShouldUpdateToken()
        {
            var token = this.Response.Context.Request.Session["access_token"];
            ((TokenResource)token).access_token.Should().Be("new token");
        }

        [Test]
        public void ShouldRedirect()
        {
            this.Response.ShouldHaveRedirectedTo("/tax/view-obligations");
        }
    }
}
