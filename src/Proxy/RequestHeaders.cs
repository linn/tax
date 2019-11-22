﻿namespace Linn.Tax.Proxy
{
    using System.Collections.Generic;

    using Linn.Common.Configuration;

    public static class RequestHeaders
    {
        public static IDictionary<string, string[]> JsonHeaders()
        {
            return new Dictionary<string, string[]> { { "Accept", new[] { "application/json" } }, { "Content-Type", new[] { "application/json" } } };
        }

        public static IDictionary<string, string[]> JsonGetHeaders()
        {
            return new Dictionary<string, string[]>
                       {
                           { "Accept", new[] { "application/vnd.hmrc.1.0+json" } }
                       };
        }

        public static IDictionary<string, string[]> JsonGetHeadersWithAppAuth(string token)
        {
            return new Dictionary<string, string[]>
                       {
                           { "Accept", new[] { "application/vnd.hmrc.1.0+json" } },
                           // todo - is this session specific
                           { "Authorization", new[] { $"Bearer {token}" } } 
                       };
        }

        public static IDictionary<string, string[]> JsonPostHeadersWithAppAuth(string token)
        {
            return new Dictionary<string, string[]>
                       {
                           { "Accept", new[] { "application/vnd.hmrc.1.0+json" } },
                           { "Authorization", new[] { $"Bearer {token}" } },
                           { "Content-Type", new[] { "application/json" } }
                       };
        }
    }
}