using System;
using System.Threading.Tasks;
using IceCreamFunction.Functions;
using Microsoft.Azure.Cosmos;

namespace IceCreamFunction.ExternalDependencies
{
    public class DbClient
    {
        private readonly CosmosClient _client;
        
        public DbClient()
        {
            _client = new CosmosClient("AccountEndpoint=https://icecreamhold7.documents.azure.com:443/;AccountKey=P7GIegk8VwEsQrKd5oly1otVaJohskoXNHOqls62DTkzdMdfL3dC3rm1qJ89xCJYVjMLWcUPOsbiNUsDDhRlhQ==;", new CosmosClientOptions
            {
                ApplicationName = "IceCreamFunction",
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

        private async Task<Container> GetContainerAsync()
        {
            var databaseResponse = await EnsureDatabaseExistenceAsync();
            var containerResponse = await EnsureContainerExistenceAsync(databaseResponse);

            return containerResponse.Container;
        }

        private async Task<ContainerResponse> EnsureContainerExistenceAsync(DatabaseResponse databaseResponse)
        {
            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties("users", "/users"));

            return containerResponse;
        }

        private async Task<DatabaseResponse> EnsureDatabaseExistenceAsync()
        {
            return await _client.CreateDatabaseIfNotExistsAsync("test12345");
        }
    }
}