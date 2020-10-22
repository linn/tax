namespace Linn.Tax.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Tax.Resources;

    public static class RequestHeaders
    {
        public static IDictionary<string, string[]> JsonGetHeaders()
        {
            return new Dictionary<string, string[]>
                       {
                           { "Accept", new[] { "application/vnd.hmrc.1.0+json" } }
                       };
        }

        public static IDictionary<string, string[]> JsonGetHeadersWithAuth(string token, FraudPreventionMetadataResource resource, string deviceId)
        {
            return new Dictionary<string, string[]>
                       {
                           { "Accept", new[] { "application/vnd.hmrc.1.0+json" } },
                           { "Authorization", new[] { $"Bearer {token}" } },
                           { "Gov-Client-Connection-Method", new[] { "WEB_APP_VIA_SERVER" } },
                           { "Gov-Client-Browser-Do-Not-Track", new[] { $"{resource.DoNotTrack}".ToLower() } }, 
                           { "Gov-Client-Browser-JS-User-Agent", new[] { resource.UserAgentString } },
                           { "Gov-Client-Local-IPs", new[] { ToCommaSeparatedList(resource.LocalIps) } }, 
                           { "Gov-Client-Browser-Plugins", new[] { ToCommaSeparatedList(resource.BrowserPlugins) } }, 
                           { "Gov-Client-Device-ID", new[] { deviceId } },
                           { "Gov-Client-Screens", new[] { $"width={resource.ScreenWidth}&height={resource.ScreenHeight}&scaling-factor={resource.ScalingFactor}&colour-depth={resource.ColourDepth}" } },
                           { "Gov-Client-Timezone", new[] { $"UTC{ToUtcString(resource.TimezoneOffset)}" } },
                           { "Gov-Client-User-IDs", new[] { $"Linn={resource.Username}" } },
                           { "Gov-Client-Window-Size", new[] { $"width={resource.WindowWidth}&height={resource.WindowHeight}" } },
                           { "Gov-Vendor-Version", new[] { "Linn.Tax.Service.Host=v1.1.0&Linn.Tax=v1.1" } },
                           { "Gov-Client-Multi-Factor", new[] { string.Empty } },
                           { "Gov-Vendor-License-IDs", new[] { string.Empty } },
                           { "Gov-Client-Public-Port", new[] { string.Empty } },
                           { "Gov-Client-Public-IP", new[] { string.Empty } },
                           { "Gov-Vendor-Public-IP", new[] { string.Empty } },
                           { "Gov-Vendor-Forwarded", new[] { string.Empty } }
                       };
        }

        public static string ToCommaSeparatedList(IEnumerable<string> list)
        {
            var str = string.Empty;

            var enumerable = list as string[] ?? list.ToArray();
            foreach (var s in enumerable)
            {
                str += $"{Uri.EscapeDataString(s)}";
                if (enumerable.Last() != s)
                {
                    str += ",";
                }
            }

            return str;
        }

        public static string ToUtcString(int offsetInMinutes)
        {
            var str = offsetInMinutes < 0 ? "-" : "+";

            var utc = new DateTimeOffset();

            utc = utc.AddMinutes(Math.Abs(offsetInMinutes)).ToOffset(new TimeSpan(0, 0, offsetInMinutes, 0));

            return str + utc.Offset.ToString().Substring(offsetInMinutes < 0 ? 1 : 0, 5);
        }
    }
}
