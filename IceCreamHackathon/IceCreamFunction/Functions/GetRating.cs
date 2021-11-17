using System;
using System.Threading.Tasks;
using IceCreamFunction.ExternalDependencies;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace IceCreamFunction.Functions
{
    public class GetRating
    {
        private readonly DbClient _dbClient;

        public GetRating(DbClient dbClient)
        {
            _dbClient = dbClient;
        }
        
        [FunctionName("GetRating")]
        public async Task<UserRatingDto> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "GetRating/{ratingId:guid}")]
            HttpRequest req,
            Guid ratingId)
        {
            var userRating = await _dbClient.GetUserRating(ratingId);

            return userRating;
        }
    }
}