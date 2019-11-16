using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Functions.LicenseGenerator
{
    public static class RequestLicense
    {
        [FunctionName("RequestLicense")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Queue("license-requests")] IAsyncCollector<LicenceRequestDetails> orderQueue,
            ILogger log)
        {
            log.LogInformation("License request function.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<LicenceRequestDetails>(requestBody);

            request.OrderDate = DateTime.UtcNow;

            await orderQueue.AddAsync(request);

            log.LogInformation($"Order Id {request.OrderId} recieved for produst {request.ProductId} at {request.OrderDate}");

            return new OkObjectResult($"Thank you for your order. Your order id is {request.OrderId}.");
        }
    }
}
