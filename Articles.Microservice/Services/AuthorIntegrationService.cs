using Articles.Microservice.DTOs;
using System.Text.Json;

namespace Articles.Microservice.Services
{
    public class AuthorIntegrationService
    {
        private readonly HttpClient _httpClient;

        public AuthorIntegrationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExternalAuthorDto?> GetAuthorByIdAsync(int authorId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/authorsapi/{authorId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ExternalAuthorDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}