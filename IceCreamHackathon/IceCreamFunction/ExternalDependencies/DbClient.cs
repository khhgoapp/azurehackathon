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
                ApplicationName = "IceCreamFunction"
            });
        }

        public async Task CreateUserRating(UserRatingDto userRatingDto)
        {
            var container = await GetContainer();

            await container.CreateItemAsync(userRatingDto);
        }

        private async Task<Container> GetContainer()
        {
            var databaseResponse = await EnsureDatabaseExistence();
            var containerResponse = await EnsureContainerExistence(databaseResponse);

            return containerResponse.Container;
        }

        private async Task<ContainerResponse> EnsureContainerExistence(DatabaseResponse databaseResponse)
        {
            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties("users", "/users"));

            return containerResponse;
        }

        private async Task<DatabaseResponse> EnsureDatabaseExistence()
        {
            return await _client.CreateDatabaseIfNotExistsAsync("test12345");
        }
    }
}