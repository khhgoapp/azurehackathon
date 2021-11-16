using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace IceCreamFunction.Functions
{
    public static class GetProductById
    {
        [FunctionName("GetProductById")]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "GetProductById/{productId:guid}")]
            HttpRequest req,
            Guid productId)
        {
            return new OkObjectResult($"The product name for your product id {productId} is Starfruit Explosion");
        }
    }
}