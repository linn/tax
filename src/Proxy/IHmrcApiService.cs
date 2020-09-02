namespace Linn.Tax.Proxy
{
    using Linn.Common.Proxy;
    using Linn.Tax.Resources;

    public interface IHmrcApiService
    {
        IRestResponse<string> SubmitVatReturn(VatReturnRequestResource vatReturn, TokenResource token, string deviceId);

        IRestResponse<string> GetVatObligations(ObligationsRequestResource resource, TokenResource token, string deviceId);

        TokenResource ExchangeCodeForAccessToken(string code);

        TokenResource RefreshToken(string refreshToken);

        TokenResource GenerateToken();

        IRestResponse<string> TestFraudPreventionHeaders(FraudPreventionMetadataResource resource, string deviceId);
    }
}