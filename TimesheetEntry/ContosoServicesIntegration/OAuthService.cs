using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContosoServicesIntegration
{
    public class OAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _resource;
        private const string _oauthEndpoint = "https://login.windows.net/be-terna.com/oauth2/token";

        public OAuthService(HttpClient httpClient, string clientId, string clientSecret, string resource)
        {
            _httpClient = httpClient;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _resource = resource;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _oauthEndpoint);
            var body = $"resource={_resource}&grant_type=client_credentials&client_id={_clientId}&client_secret={_clientSecret}";
            request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get access token: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<OAuthTokenResponse>(content);
            return tokenResponse.AccessToken;
        }
    }

    public class OAuthTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
