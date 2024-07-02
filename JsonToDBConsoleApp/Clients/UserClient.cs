using JsonToDBConsoleApp.DTOs;
using System.Net.Http.Json;

namespace JsonToDBConsoleApp.Clients
{
    public class UserClient
    {
        private readonly HttpClient _httpClient;

        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDto>?> GetUsers()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://microsoftedge.github.io/Demos/json-dummy-data/64KB.json");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<UserDto>>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error fetching users from URL: {e.Message}");

                return null;
            }
        }
    }
}
