using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IceCreamFunctionJr.AzureFunctions;
using IceCreamFunctionJr.AzureFunctions.UserRatings;
using Microsoft.Azure.Cosmos;

namespace IceCreamFunctionJr.ExternalDependencies.UserRatings
{
    public class UserRatingsClient
    {
        private const string AccountEndPoint = "https://icecreamhold7.documents.azure.com:443/";
        private const string AccountKey = "P7GIegk8VwEsQrKd5oly1otVaJohskoXNHOqls62DTkzdMdfL3dC3rm1qJ89xCJYVjMLWcUPOsbiNUsDDhRlhQ==";
        
        private readonly CosmosClient _cosmosClient;

        public UserRatingsClient()
        {
            _cosmosClient = new CosmosClient($"AccountEndpoint={AccountEndPoint};AccountKey={AccountKey};", new CosmosClientOptions
            {
                ApplicationName = "IceCreamFunctionJr",
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });
        }

        public async Task CreateUserRating(UserRatingDto userRatingDto)
        {
            var container = await GetContainerAsync();
            await container.CreateItemAsync(userRatingDto);
        }
        
        public async Task<UserRatingDto> GetUserRating(Guid ratingId)
        {
            var container = await GetContainerAsync();

            return await container.ReadItemAsync<UserRatingDto>(ratingId.ToString(), PartitionKey.None);
        }
        
        public async Task<List<UserRatingDto>> GetUserRatings(Guid userId)
        {
            var container = await GetContainerAsync();
            var iterator = container.GetItemLinqQueryable<UserRatingDto>(true);

            return iterator.Where(x => x.UserId == userId).ToList();
        }

        private async Task<Container> GetContainerAsync()
        {
            var databaseResponse = await EnsureDatabaseExistsAsync();
            var containerResponse = await EnsureContainerExistsAsync(databaseResponse);

            return containerResponse.Container;
        }

        private async Task<ContainerResponse> EnsureContainerExistsAsync(DatabaseResponse databaseResponse)
        {
            return await databaseResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties("UserRatings", "/Id"));
        }

        private async Task<DatabaseResponse> EnsureDatabaseExistsAsync()
        {
            return await _cosmosClient.CreateDatabaseIfNotExistsAsync("UserRatingsDbJrj");
        }
    }
}