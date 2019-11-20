namespace Linn.Tax.Domain
{
    public interface IHmrcApiService
    {
        string HelloWorld();

        string HelloApplication();

        string HelloUser(string token);


        string ExchangeCodeForAccessToken(string code);
    }
}