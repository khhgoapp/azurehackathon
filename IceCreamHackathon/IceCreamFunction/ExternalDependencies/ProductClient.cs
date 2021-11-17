using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceCreamFunction.ExternalDependencies
{
    public class ProductClient
    {
        private const string BaseUrl = "https://serverlessohproduct.trafficmanager.net/api";

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var httpClient = HttpClientFactory.Create();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/GetProducts");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode) return Array.Empty<ProductDto>();

            var result = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();

            return result;
        }

        public async Task<ProductDto?> GetProductAsync(Guid productId)
        {
            var httpClient = HttpClientFactory.Create();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/GetProduct?productId={productId}");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsAsync<ProductDto>();
        }
    }

    public record ProductDto(Guid ProductId, string ProductName, string ProductDescription);
}