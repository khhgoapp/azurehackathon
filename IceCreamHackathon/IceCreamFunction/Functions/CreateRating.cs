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

        public CreateRating(ProductClient productClient, UserClient userClient)
        {
            _productClient = productClient;
            _userClient = userClient;
        }
        
        [FunctionName("CreateRating")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] CreateRatingRequest createRating)
        {
            var user = await UserClient.GetUserAsync(createRating.UserId);

            if (user is null) return new BadRequestObjectResult("No user with the id provided was not found.");
            
            var product = await ProductClient.GetProductAsync(createRating.ProductId);
            
            if (product is null) return new BadRequestObjectResult("No product with the id provided was not found.");
            
            
            
            return new OkObjectResult($"{createRating}");
        }
    }
}