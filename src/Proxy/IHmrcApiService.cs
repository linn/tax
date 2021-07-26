namespace Linn.Tax.Proxy
{
    using Linn.Common.Proxy;
    using Linn.Tax.Resources;

    public interface IHmrcApiService
    {
        IRestResponse<string> SubmitVatReturn(VatReturnSubmissionResource vatReturn, TokenResource token);

        IRestResponse<string> GetVatObligations(ObligationsRequestResource resource, TokenResource token);

        TokenResource ExchangeCodeForAccessToken(string code);

        TokenResource RefreshToken(string refreshToken);

        IRestResponse<string> TestFraudPreventionHeaders(FraudPreventionMetadataResource resource);
    }
}
