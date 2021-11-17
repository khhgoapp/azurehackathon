using System;
using System.IO;
using System.Threading.Tasks;
using IceCreamFunctionJr.ExternalDependencies.Products;
using IceCreamFunctionJr.ExternalDependencies.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IceCreamFunctionJr.AzureFunctions
{
    public record UserRatingDto(
        Guid Id,
        Guid UserId,
        Guid ProductId,
        DateTime Timestamp,
        string LocationName,
        int Rating,
        string UserNotes);
    
    public class CreateRating
    {
        private readonly ILogger<CreateRating> _logger;
        private readonly ProductClient _productClient;
        private readonly UserClient _userClient;

        public CreateRating(ILogger<CreateRating> log, ProductClient productClient, UserClient userClient)
        {
            _logger = log;
            _productClient = productClient;
            _userClient = userClient;
        }
        
        [FunctionName(nameof(CreateRating))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (!Guid.TryParse(GetParameter("userId", req, data), out Guid userId))
                return new BadRequestObjectResult($"{nameof(userId)} parameter is missing or not a valid GUID.");

            var (userResponseStatusMsg, user) = await _userClient.GetUserAsync(userId);
            if (userResponseStatusMsg != null)
                return new BadRequestObjectResult(userResponseStatusMsg);
            
            if (!Guid.TryParse(GetParameter("productId", req, data), out Guid productId))
                return new BadRequestObjectResult($"{nameof(productId)} parameter is missing or not a valid GUID.");
            
            var (productResponseStatusMsg, product) = await _productClient.GetProductAsync(productId);
            if (productResponseStatusMsg != null)
                return new BadRequestObjectResult(productResponseStatusMsg);

            var locationName = GetParameter("locationName", req, data);
            if (string.IsNullOrWhiteSpace(locationName))
                return new BadRequestObjectResult($"{nameof(locationName)} parameter is missing or not a valid GUID.");
            
            if (!int.TryParse(GetParameter("rating", req, data), out int rating))
                return new BadRequestObjectResult($"{nameof(rating)} parameter is missing or not a valid GUID.");
            
            if (rating is < 0 or > 5) 
                return new BadRequestObjectResult("Rating must be an integer between 0 and 5");
            
            var userNotes = GetParameter("userNotes", req, data);
            if (string.IsNullOrWhiteSpace(userNotes))
                return new BadRequestObjectResult($"{nameof(userNotes)} parameter is missing or not a valid GUID.");
            
            var result = new UserRatingDto(Guid.NewGuid(), userId, productId, DateTime.UtcNow, locationName, rating, userNotes);
            return new OkObjectResult(result);
        }
        
        private string GetParameter(string parameterName, HttpRequest request, dynamic data)
        {
            string parameter = request.Query[parameterName];
            parameter ??= data.name;

            return parameter;
        }
    }
}