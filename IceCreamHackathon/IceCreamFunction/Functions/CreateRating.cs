using System;
using System.Threading.Tasks;
using IceCreamFunction.ExternalDependencies;
using IceCreamFunction.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace IceCreamFunction.Functions
{
    public class CreateRating
    {
        private readonly ProductClient _productClient;
        private readonly UserClient _userClient;
        private readonly DbClient _dbClient;

        public CreateRating(ProductClient productClient, UserClient userClient, DbClient dbClient)
        {
            _productClient = productClient;
            _userClient = userClient;
            _dbClient = dbClient;
        }
        
        [FunctionName("CreateRating")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = null)]
            CreateRatingRequest createRating)
        {
            var user = await _userClient.GetUserAsync(createRating.UserId);

            if (user is null) return new BadRequestObjectResult("No user with the id provided was not found.");
            
            var product = await _productClient.GetProductAsync(createRating.ProductId);
            
            if (product is null) return new BadRequestObjectResult("No product with the id provided was not found.");

            if (createRating.Rating is < 0 or > 5) return new BadRequestObjectResult("Rating must be an integer between 0 and 5");

            var result = new UserRatingDto(Guid.NewGuid(), createRating.UserId, createRating.ProductId, DateTime.UtcNow, createRating.LocationName, createRating.Rating, createRating.UserNotes);

            await _dbClient.CreateUserRating(result);
            
            return new OkObjectResult(result);
        }
    }

    public record UserRatingDto(
        Guid id,
        Guid UserId,
        Guid ProductId,
        DateTime Timestamp,
        string LocationName,
        int Rating,
        string UserNotes);
}