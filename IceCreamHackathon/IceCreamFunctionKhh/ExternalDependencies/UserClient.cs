using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace IceCreamFunctionKhh.ExternalDependencies
{
    public class UserClient
    {
        private const string BaseUrl = "https://serverlessohuser.trafficmanager.net/api";

        public static async Task<UserDto?> GetUserAsync(Guid userId)
        {
            var httpClient = HttpClientFactory.Create();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/GetUser?userId={userId}");
            
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsAsync<UserDto>();
        }
    }

    public record UserDto(Guid UserId, string UserName, string FullName);
}