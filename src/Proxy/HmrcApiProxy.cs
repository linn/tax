﻿namespace Linn.Tax.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Net;
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

        public IRestResponse<string> SubmitVatReturn(VatReturnRequestResource resource, TokenResource token)
        {
            var json = new JsonSerializer();
            var uri = new Uri($"{this.rootUri}organisations/vat/{resource.Vrn}/returns", UriKind.RelativeOrAbsolute);
            return this.restClient.Post(
                CancellationToken.None,
                uri,
                new Dictionary<string, string>(),
                RequestHeaders.JsonGetHeadersWithAppAuth(token.access_token),
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
        }

        public TokenResource ExchangeCodeForAccessToken(string code)
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

            return response.Value;
        }

        public TokenResource RefreshToken(string refreshToken)
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
                        grant_type = "refresh_token",
                        refresh_token = refreshToken
                }).Result;

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new AccessTokenExpiredException("Access token expired.");
            }

            return response.Value;
        }
    }
}