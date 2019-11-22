namespace Linn.Tax.Proxy
{
    using Linn.Tax.Resources;

    public interface IHmrcApiService
    {
        VatReturnResponseResource SubmitVatReturn(string token, VatReturnRequestResource vatReturn);

        string ExchangeCodeForAccessToken(string code);
    }
}