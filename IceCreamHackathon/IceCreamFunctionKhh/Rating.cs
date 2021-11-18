using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using IceCreamFunctionKhh;
using IceCreamFunctionKhh.ExternalDependencies;
using IceCreamFunctionKhh.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace VSFunctionTemplate
{
    public class Rating
    {
        private readonly ProductClient _productClient;
        private readonly UserClient _userClient;
        private readonly ILogger<Rating> _logger;
        private const string _dbName = "RatingsDBKHH";
        private const string _collectionName = "Ratings";

        public Rating(ILogger<Rating> log, ProductClient productClient, UserClient userClient)
        {
            _logger = log;
            _productClient = productClient;
            _userClient = userClient;
        }

        [FunctionName("CreateRatings")]
        //[OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        //[OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = "Ratings")] CreateRatingRequest createRating,
            [CosmosDB(
                databaseName: _dbName,
                collectionName: _collectionName,
                CreateIfNotExists = true,
                ConnectionStringSetting = "CosmosDBConnectionString")] out dynamic document)
        {
            _logger.LogInformation("Create ratings HTTP trigger");

            var user = UserClient.GetUserAsync(createRating.UserId).Result;

            if (user is null)
            {
                document = null;
                return new BadRequestObjectResult($"No user with the id: {createRating.UserId} was found.");
            }

            var product = ProductClient.GetProductAsync(createRating.ProductId).Result;

            if (product is null)
            {
                document = null;
                return new BadRequestObjectResult($"No product with the id: {createRating.ProductId} provided was found.");
            }

            document = createRating;
            return new OkObjectResult($"Rating: {createRating.id} created");
        }

        [FunctionName("GetRating")]
        public IActionResult GetRating(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "Rating/{ratingId:guid}")]
                HttpRequest req,
                Guid ratingId,
            [CosmosDB(
                databaseName: _dbName,
                collectionName: _collectionName,
                ConnectionStringSetting = "CosmosDBConnectionString",
                Id = "{ratingId}",
                PartitionKey = "{ratingId}")] RatingDocument doc)
        {
            return new OkObjectResult(doc);
        }
    }
}

