namespace Linn.Tax.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;

    using Linn.Common.Proxy;
    using Linn.Tax.Resources;

    using Newtonsoft.Json;

    using JsonSerializer = Linn.Common.Serialization.Json.JsonSerializer;

    public class HmrcApiProxy : IHmrcApiService
    {

        private readonly IRestClient restClient;

        private readonly string rootUri;

        private readonly string serverToken;

        private readonly string clientId;

        private readonly string clientSecret;


        public HmrcApiProxy(IRestClient restClient, string rootUri, string serverToken, string clientId, string clientSecret)
        {
            this.rootUri = rootUri;
            this.serverToken = serverToken;
            this.restClient = restClient;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        private string Token
        {
            get; set;
        }


        public string HelloWorld()
        {
            var uri = new Uri($"{this.rootUri}/hello/world", UriKind.RelativeOrAbsolute);
            var response = this.restClient.Get(
                CancellationToken.None,
                uri,
                new Dictionary<string, string>(),
                RequestHeaders.JsonGetHeaders()).Result;

            var json = new JsonSerializer();
            var resource = json.Deserialize<Dictionary<string, string>>(response.Value);
            return resource["message"];
        }

        public string HelloApplication()
        {
            var uri = new Uri($"{this.rootUri}/hello/application", UriKind.RelativeOrAbsolute);
            var response = this.restClient.Get(
                CancellationToken.None,
                uri,
                new Dictionary<string, string>(),
                RequestHeaders.JsonGetHeadersWithAppAuth(this.serverToken)).Result;

            var json = new JsonSerializer();
            var resource = json.Deserialize<Dictionary<string, string>>(response.Value);
            return resource["message"];
        }

        public string HelloUser(string token)
        {
            var uri = new Uri($"{this.rootUri}/hello/user", UriKind.RelativeOrAbsolute);
            var response = this.restClient.Get(
                CancellationToken.None,
                uri,
                new Dictionary<string, string>(),
                RequestHeaders.JsonGetHeadersWithAppAuth(token)).Result;

            var json = new JsonSerializer();
            var resource = json.Deserialize<Dictionary<string, string>>(response.Value);
            return resource["message"];
        }

        public string SubmitVatReturn(string token, VatReturnRequestResource body)
        {
            var uri = new Uri($"{this.rootUri}organisations/vat/465101180/returns", UriKind.RelativeOrAbsolute);
            var response = this.restClient.Post(
                CancellationToken.None,
                uri,
                new Dictionary<string, string>(),
                RequestHeaders.JsonGetHeadersWithAppAuth(token),
               new JsonSerializer().Serialize(body),
                "application/json").Result;

            return response.Value;
        }

        public string ExchangeCodeForAccessToken(string code)
        {
            var uri = new Uri($"{this.rootUri}/oauth/token", UriKind.RelativeOrAbsolute);
            var response = this.restClient.Post<TokenResource>(
                CancellationToken.None,
                uri,
                new Dictionary<string, string>(),
                RequestHeaders.JsonGetHeaders(),
                new
                    {
                        client_id = this.clientId,
                        client_secret = this.clientSecret,
                        grant_type = "authorization_code",
                        redirect_uri = "http://localhost:61798/tax/redirect",
                        code
                    }).Result;

            return response.Value.access_token;
        }

        internal class TokenResource
        {
            public string access_token { get; set; }

            public string token_type { get; set; }

            public int expires_in { get; set; }

            public string refresh_token { get; set; }
        }
    }
}