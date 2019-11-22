namespace Linn.Tax.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Linn.Common.Configuration;
    using Linn.Common.Proxy;
    using Linn.Tax.Resources;
    using JsonSerializer = Common.Serialization.Json.JsonSerializer;

    public class HmrcApiProxy : IHmrcApiService
    {
        private readonly IRestClient restClient;

        private readonly string rootUri;

        private readonly string clientId;

        private readonly string clientSecret;


        public HmrcApiProxy(IRestClient restClient, string rootUri, string clientId, string clientSecret)
        {
            this.rootUri = rootUri;
            this.restClient = restClient;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public VatReturnResponseResource SubmitVatReturn(string token, VatReturnRequestResource resource)
        {
            var json = new JsonSerializer();
            var uri = new Uri($"{this.rootUri}organisations/vat/{resource.Vrn}/returns", UriKind.RelativeOrAbsolute);
            var response = this.restClient.Post(
                CancellationToken.None,
                uri,
                new Dictionary<string, string>(),
                RequestHeaders.JsonGetHeadersWithAppAuth(token),
                json.Serialize(new
                                   {
                                        periodKey = resource.PeriodKey,
                                        vatDueSales = resource.VatDueSales,
                                        vatDueAcquisitions = resource.VatDueAcquisitions,
                                        totalVatDue = resource.TotalVatDue,
                                        vatReclaimedCurrPeriod = resource.VatReclaimedCurrPeriod,
                                        netVatDue = resource.NetVatDue,
                                        totalValueSalesExVAT = resource.TotalValueSalesExVat,
                                        totalValuePurchasesExVAT = resource.TotalValuePurchasesExVat,
                                        totalValueGoodsSuppliedExVAT = resource.TotalValueGoodsSuppliedExVat,
                                        totalAcquisitionsExVAT = resource.TotalAcquisitionsExVat,
                                        finalised = resource.Finalised
                                   }),
                "application/json").Result;

            // todo - check for status code here and formulate the correct response
            var result = json.Deserialize<VatReturnResponseResource>(response.Value);
            return result;
        }

        public string ExchangeCodeForAccessToken(string code)
        {
            var uri = new Uri($"{this.rootUri}/oauth/token", UriKind.RelativeOrAbsolute);
            var redirect = ConfigurationManager.Configuration["AUTH_CALLBACK_URI"];
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
                        redirect_uri = redirect,
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