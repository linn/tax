namespace Linn.Tax.Proxy
{
    using System;
    using System.Collections.Generic;

    public static class RequestHeaders
    {
        public static IDictionary<string, string[]> JsonGetHeaders()
        {
            return new Dictionary<string, string[]>
                       {
                           { "Accept", new[] { "application/vnd.hmrc.1.0+json" } }
                       };
        }

        public static IDictionary<string, string[]> JsonGetHeadersWithAuth(string token)
        {
            return new Dictionary<string, string[]>
                       {
                           { "Accept", new[] { "application/vnd.hmrc.1.0+json" } },
                           { "Authorization", new[] { $"Bearer {token}" } },
                           { "Gov-Client-Connection-Method", new[] { "WEB_APP_VIA_SERVER" } },
                           { "Gov-Client-Browser-Do-Not-Track", new[] { "false" } }, // todo - get via jss?,
                           // todo - get via js
                           { "Gov-Client-Browser-JS-User-Agent", new[] { "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36" } },

                           { "Gov-Client-Local-IPs", new[] { Uri.EscapeDataString("fe80::9cab:9308:1e04:e801%4") } }, // todo - get somehow (RTC)
                           { "Gov-Client-Browser-Plugins", new[] { string.Empty } }, // ?
                           { "Gov-Client-Device-ID", new[] { "b121e704-007a-4ba8-8845-d92905dc3665" } }, // todo - store
                           //{ "Gov-Client-Public-IP", new[] { string.Empty } }, // todo - get?
                           { "Gov-Client-Public-Port", new[] { "" } },
                           // todo - find out?
                           { "Gov-Client-Screens", new[] { "width=1920&height=1080&scaling-factor=1&colour-depth=16,width=3000&height=2000&scaling-factor=1.25&colour-depth=16" } },
                           { "Gov-Client-Timezone", new[] { "UTC+00:00" } },
                           { "Gov-Client-User-IDs", new[] { "my-vendor=alice_online_account_user_id_with_vendor&my-secondary-vendor=alice_online_account_user_id_with_secondary_vendor" } }, // ?
                           { "Gov-Client-Window-Size", new[] { "width=1256&height=803" } }, // todo - get via js
                           { "Gov-Vendor-Forwarded", new[] { string.Empty } }, // ?
                           { "Gov-Vendor-Public-IP", new[] { Uri.EscapeDataString("195.59.102.251") } }, // todo - server ip?
                           { "Gov-Vendor-Version", new[] { "my-frontend-app=2.2.2&my-serverside-code=v3.8" } }, // todo - what??
                           { "Gov-Client-Multi-Factor", new[] { "type=TOTP&timestamp=2017-04-21T13%3A23%3A42Z&unique-reference=c672b8d1ef56ed28" } },
                           { "Gov-Vendor-License-IDs", new[] { string.Empty,  } }
                       };
        }
    }
}