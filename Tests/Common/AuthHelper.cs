using Domain.Transfer;
using System.Net.Http.Json;

namespace Tests.Common
{
    public class AuthHelper
    {
        private readonly HttpClient _client;
        private TokenDTO _cachedToken;

        public AuthHelper(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetTokenAsync(string username = "admin", string password = "AdminPassword123")
        {
            if (_cachedToken != null && !string.IsNullOrEmpty(_cachedToken.Token))
                return _cachedToken.Token;

            var userDto = new LoginDTO { Username = username, Password = password };

            var response = await _client.PostAsJsonAsync("/api/auth/login", userDto);

            response.EnsureSuccessStatusCode();

            _cachedToken = await response.Content.ReadFromJsonAsync<TokenDTO>();

            return _cachedToken.Token;
        }
    }
}
