using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace IceCreamFunction.Functions
{
    public static class GetProductById
    {
        [Function("GetProductById")]
        public static HttpResponseData Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "GetProductById/{productId}")]
            HttpRequestData req,
            string productId,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetProductById");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString($"The product name for your product id {productId} is Starfruit Explosion");

            return response;
        }
    }
}