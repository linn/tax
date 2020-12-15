namespace Linn.Tax.Proxy
{
    using System;

    public class AccessTokenExpiredException : Exception
    {
        public AccessTokenExpiredException(string message)
            : base(message)
        {
        }

        public AccessTokenExpiredException()
        {
        }
    }
}
