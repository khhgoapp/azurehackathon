using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceCreamFunctionJr.ExternalDependencies.Products
{
    public class ProductClient
    {
        private const string BaseUrl = "https://serverlessohproduct.trafficmanager.net";

        public async Task<(string? responseStatusMsg, ProductDto? product)> GetProductAsync(Guid productId)
        {
            var httpClient = HttpClientFactory.Create();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/GetProduct?productId={productId}");
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) 
                return ($"Product with productId {productId} not found at {BaseUrl}", null);

            return (null, await response.Content.ReadAsAsync<ProductDto>());
        }
    }
}