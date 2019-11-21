namespace Linn.Tax.Proxy
{
    using Linn.Tax.Resources;

    public interface IHmrcApiService
    {
        string SubmitVatReturn(string token, VatReturnRequestResource vatReturn);

        string ExchangeCodeForAccessToken(string code);
    }
}