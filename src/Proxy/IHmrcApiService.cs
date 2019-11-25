namespace Linn.Tax.Proxy
{
    using Linn.Common.Proxy;
    using Linn.Tax.Resources;

    public interface IHmrcApiService
    {
        IRestResponse<string> SubmitVatReturn(VatReturnRequestResource vatReturn, string token);

        string ExchangeCodeForAccessToken(string code);
    }
}