using System;

namespace Functions.LicenseGenerator
{
    public class LicenceRequestDetails
    {
        public int OrderId { get; set; }
        
        public string ProductId { get; set; }

        public DateTime OrderDate { get; set; }

        public string Email { get; set; }
    }
}