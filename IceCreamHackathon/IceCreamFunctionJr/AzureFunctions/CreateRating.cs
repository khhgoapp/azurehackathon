using System;
using System.Threading.Tasks;
using IceCreamFunctionJr.ExternalDependencies.Products;
using IceCreamFunctionJr.ExternalDependencies.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] CreateRatingRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            
            var (userResponseStatusMsg, user) = await _userClient.GetUserAsync(req.UserId);
            if (userResponseStatusMsg != null)
                return new BadRequestObjectResult(userResponseStatusMsg);
            
            var (productResponseStatusMsg, product) = await _productClient.GetProductAsync(req.ProductId);
            if (productResponseStatusMsg != null)
                return new BadRequestObjectResult(productResponseStatusMsg);

            if (req.Rating is < 0 or > 5) 
                return new BadRequestObjectResult("Rating must be an integer between 0 and 5");
            
            var result = new UserRatingDto(Guid.NewGuid(), req.UserId, req.ProductId, DateTime.UtcNow, req.LocationName, req.Rating, req.UserNotes);
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