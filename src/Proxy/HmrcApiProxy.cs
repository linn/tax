namespace Linn.Tax.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Linn.Common.Proxy;
    using Linn.Common.Serialization.Json;
    using Linn.Tax.Domain;

    public class HmrcApiProxy : IHmrcApiService
    {

        private readonly IRestClient restClient;

        private readonly string rootUri;

        public HmrcApiProxy(IRestClient restClient, string rootUri)
        {
            this.rootUri = rootUri;
            this.restClient = restClient;
        }

        public string HelloWorld()
        {
            var uri = new Uri($"{this.rootUri}/hello/world", UriKind.RelativeOrAbsolute);
            var response = this.restClient.Get(
                CancellationToken.None,
                uri,
                new Dictionary<string, string>(),
                DefaultHeaders.JsonGetHeaders()).Result;

            var json = new JsonSerializer();
            var resource = json.Deserialize<Dictionary<string, string>>(response.Value);
            return resource["message"];
        }
    }
}