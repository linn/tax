namespace Linn.Tax.Service.Tests.AuthModuleSpecs
{
    using System.Threading.Tasks;

    using Linn.Tax.Proxy;
    using Linn.Tax.Resources;
    using Nancy.Testing;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

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