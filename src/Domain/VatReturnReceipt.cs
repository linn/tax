using System;

namespace Linn.Tax.Domain
{
    class VatReturnReceipt
    {
        public class VatReturnReceiptResource
        {
            public DateTime ProcessingDate { get; set; }

            public string PaymentIndicator { get; set; }

            public int FormBundleNumber { get; set; }

            public string ChargeRefNumber { get; set; }
        }
    }
}
