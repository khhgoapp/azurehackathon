using System;
using System.Threading.Tasks;
using IceCreamFunctionJr.ExternalDependencies.Products;
using IceCreamFunctionJr.ExternalDependencies.UserRatings;
using IceCreamFunctionJr.ExternalDependencies.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace IceCreamFunctionJr.AzureFunctions.UserRatings
{
    public record UserRatingDto(
        Guid Id,
        Guid UserId,
        Guid ProductId,
        DateTime Timestamp,
        string LocationName,
        int Rating,
        string UserNotes);
    
    public class UserRatingFunctions
    {
        private readonly ILogger<UserRatingFunctions> _logger;
        private readonly ProductClient _productClient;
        private readonly UserClient _userClient;
        private readonly UserRatingsClient _userRatingsClient;

        public UserRatingFunctions(ILogger<UserRatingFunctions> log, ProductClient productClient, UserClient userClient, UserRatingsClient userRatingsClient)
        {
            _logger = log;
            _productClient = productClient;
            _userClient = userClient;
            _userRatingsClient = userRatingsClient;
        }
        
        [FunctionName(nameof(CreateRating))]
        public async Task<IActionResult> CreateRating(
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
            
            var userRating = new UserRatingDto(Guid.NewGuid(), req.UserId, req.ProductId, DateTime.UtcNow, req.LocationName, req.Rating, req.UserNotes);
            await _userRatingsClient.CreateUserRating(userRating);
            
            return new OkObjectResult(userRating);
        }
    }
}