using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Functions.LicenseGenerator
{
    public static class QueueTrigger
    {
        [FunctionName("GenerateLicense")]
        public static void Run([QueueTrigger("license-requests")] LicenceRequestDetails licenseRequest,
        [Blob("licenses/{rand-guid}.lic")] TextWriter outputBlob,
        ILogger log)
        {
            outputBlob.WriteLine($"OrderId: {licenseRequest.OrderId}");
            outputBlob.WriteLine($"ProductId: {licenseRequest.ProductId}");
            outputBlob.WriteLine($"OrderDate: {licenseRequest.OrderDate}");
            outputBlob.WriteLine($"Email: {licenseRequest.Email}");

            outputBlob.WriteLine($"License Key: {Guid.NewGuid()}");

            log.LogInformation($"C# Queue trigger function processed: {licenseRequest}");
        }
    }
}