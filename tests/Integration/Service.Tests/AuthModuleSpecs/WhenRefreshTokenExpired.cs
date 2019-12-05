namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Tax.Proxy;
    using Nancy.Testing;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRefreshTokenExpired : AccessTokenContext
    {
        [SetUp]
        public void SetUp()
        {

            this.ApiService.RefreshToken(Arg.Any<string>()).Returns(x => throw new AccessTokenExpiredException("e"));
            this.Browser.Get(
                "/auth",
                with => { with.Header("Accept", "application/json"); });

            this.Response = this.Browser.Get(
                "/auth",
                with =>
                    {
                        with.Header("Accept", "application/json");
                    }).Result;
        }

        [Test]
        public void ShouldClearOutExpiredToken()
        {
            this.Response.Context.Request.Session["access_token"].Should().Be(null);
        }

        [Test]
        public void ShouldCallApi()
        {
            this.ApiService.Received().RefreshToken(Arg.Any<string>());
        }

        [Test]
        public void ShouldRedirect()
        {
            this.Response.ShouldHaveRedirectedTo("/auth");
        }
    }
}