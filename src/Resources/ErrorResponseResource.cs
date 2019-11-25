namespace Linn.Tax.Resources
{
    using System.Collections.Generic;

    public class ErrorResponseResource
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public IEnumerable<ErrorResponseResource> Errors { get; set; }
    }
}