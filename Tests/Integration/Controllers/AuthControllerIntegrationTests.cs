using Domain.Transfer;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Tests.Integration.Controllers
{
    public  class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ShouldReturnTokenForValidCredentials()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Username = "admin",
                Password = "AdminPassword123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenDTO>();

            Assert.NotNull(tokenResponse);
            Assert.False(string.IsNullOrEmpty(tokenResponse.Token));
        }
    }
}
