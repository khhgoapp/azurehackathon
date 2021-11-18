using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace IceCreamFunctionJr.AzureFunctions.Products
{
    public class GetProductName
    {
        private readonly ILogger<GetProductName> _logger;

        public GetProductName(ILogger<GetProductName> log)
        {
            _logger = log;
        }
        
        [FunctionName(nameof(GetProductName))]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = nameof(GetProductName) + "/{productId}")] HttpRequest req,
            Guid productId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var responseMessage = $"The product name for your product id {productId} is Starfruit Explosion";

            return new OkObjectResult(responseMessage);
        }
    }
}
