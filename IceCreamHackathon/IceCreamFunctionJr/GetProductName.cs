using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace IceCreamFunctionJr
{
    public class GetProductName
    {
        private readonly ILogger<GetProductName> _logger;

        public GetProductName(ILogger<GetProductName> log)
        {
            _logger = log;
        }
        
        private Guid _productId = Guid.Parse("AD9818DE-EAFB-46DD-85AB-B5252E4D3168");

        [FunctionName(nameof(GetProductName))]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
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
