using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceCreamFunctionJr.ExternalDependencies.Users
{
    public class UserClient
    {
        private const string BaseUrl = "https://serverlessohuser.trafficmanager.net";

        public async Task<(string? responseStatusMsg, UserDto? user)> GetUserAsync(Guid userId)
        {
            var httpClient = HttpClientFactory.Create();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/GetUser?userId={userId}");
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) 
                return ($"User with userId {userId} not found at {BaseUrl}", null);

            return (null, await response.Content.ReadAsAsync<UserDto>());
        }
    }
}