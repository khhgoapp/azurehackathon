using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IceCreamFunction.ExternalDependencies;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace IceCreamFunction.Functions
{
    public class GetRatings
    {
        private readonly DbClient _dbClient;

        public GetRatings(DbClient dbClient)
        {
            _dbClient = dbClient;
        }
        
        [FunctionName("GetRatings")]
        public async Task<List<UserRatingDto>> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get")]
            HttpRequest req)
        {
            var userRatings = await _dbClient.GetUserRatings();

            return userRatings;
        }
    }
}