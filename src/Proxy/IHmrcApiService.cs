﻿namespace Linn.Tax.Proxy
{
    using Linn.Tax.Resources;

    public interface IHmrcApiService
    {
        VatReturnResponseResource SubmitVatReturn(VatReturnRequestResource vatReturn, string token);

        string ExchangeCodeForAccessToken(string code);
    }
}