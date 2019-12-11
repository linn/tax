namespace Linn.Tax.Proxy.Tests.RequestHeadersSpecs
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenConvertingMinutesOffsetToUtcString
    {
        [Test]
        public void ShouldHandlePositiveOffset()
        {
            RequestHeaders.ToUtcString(15).Should().Be("+00:15");
            RequestHeaders.ToUtcString(60).Should().Be("+01:00");
            RequestHeaders.ToUtcString(75).Should().Be("+01:15");
            RequestHeaders.ToUtcString(180).Should().Be("+03:00");
        }

        [Test]
        public void ShouldHandleNegativeOffset()
        {
            RequestHeaders.ToUtcString(-15).Should().Be("-00:15");
            RequestHeaders.ToUtcString(-60).Should().Be("-01:00");
            RequestHeaders.ToUtcString(-75).Should().Be("-01:15");
            RequestHeaders.ToUtcString(-180).Should().Be("-03:00");
        }

        [Test]
        public void ShouldHandleZeroOffset()
        {
            RequestHeaders.ToUtcString(0).Should().Be("+00:00");
           RequestHeaders.ToUtcString(-0).Should().Be("+00:00");
        }
    }
}