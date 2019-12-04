namespace Linn.Tax.Proxy
{
    using System;
    using System.Collections.Generic;

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

        public static IDictionary<string, string[]> JsonGetHeadersWithAuth(string token, VatReturnRequestResource resource, string deviceId)
        {
            return new Dictionary<string, string[]>
                       {
                           { "Accept", new[] { "application/vnd.hmrc.1.0+json" } },
                           { "Authorization", new[] { $"Bearer {token}" } },
                           { "Gov-Client-Connection-Method", new[] { "WEB_APP_VIA_SERVER" } },
                           { "Gov-Client-Browser-Do-Not-Track", new[] { $"{resource.DoNotTrack}" } }, 
                           { "Gov-Client-Browser-JS-User-Agent", new[] { resource.UserAgentString } },
                           { "Gov-Client-Local-IPs", new[] { ToCommaSeparatedList(resource.LocalIps) } }, 
                           { "Gov-Client-Browser-Plugins", new[] { ToCommaSeparatedList(resource.BrowserPlugins) } }, 
                           { "Gov-Client-Device-ID", new[] { deviceId } }, 
                           //// { "Gov-Client-Public-IP", new[] { string.Empty } }, omitted
                           //// { "Gov-Client-Public-Port", new[] { "" } },  omitted
                           { "Gov-Client-Screens", new[] { $"width={resource.ScreenWidth}&height={resource.ScreenHeight}&scaling-factor={resource.ScalingFactor}&colour-depth=16" } },
                           { "Gov-Client-Timezone", new[] { $"UTC+00:00" } }, // hardcoded for now, but resource does provide offset as a number of minutes
                           { "Gov-Client-User-IDs", new[] { $"Linn={resource.Username}" } },
                           { "Gov-Client-Window-Size", new[] { $"width={resource.WindowWidth}&height=${resource.WindowHeight}" } },
                           //// { "Gov-Vendor-Forwarded", new[] { string.Empty } },  omitted
                           //// { "Gov-Vendor-Public-IP", new[] { Uri.EscapeDataString("195.59.102.251") } }, omitted
                           { "Gov-Vendor-Version", new[] { "Linn.Tax.Service.Host=v1.0.0&Linn.Tax=v1.0" } },
                           ///// { "Gov-Client-Multi-Factor", new[] { "type=TOTP&timestamp=2017-04-21T13%3A23%3A42Z&unique-reference=c672b8d1ef56ed28" } }, omitted
                           //// { "Gov-Vendor-License-IDs", new[] { string.Empty,  } } omitted
                       };
        }

        private static string ToCommaSeparatedList(IEnumerable<string> list)
        {
            var str = string.Empty;

            foreach (var s in list)
            {
                str += $"{Uri.EscapeDataString(s)},";
            }

            return str;
        } 
    }
}