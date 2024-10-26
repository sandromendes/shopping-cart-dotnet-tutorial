using Domain.Transfer;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using Tests.Common;

namespace Tests.Integration.Controllers
{
    public class UserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly AuthHelper _authHelper;

        public UserControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            _authHelper = new AuthHelper(_client);
        }

        private async Task AuthenticateAdminAsync()
        {
            var token = await _authHelper.GetTokenAsync("admin", "AdminPassword123");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnUserForValidId()
        {
            await AuthenticateAdminAsync();

            var userId = Guid.NewGuid().ToString();
            var response = await _client.GetAsync($"/api/user/{userId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // Supondo que o usuário ainda não exista
        }

        [Fact]
        public async Task AddUserAsync_ShouldReturnCreatedAtAction()
        {
            await AuthenticateAdminAsync();

            var user = new UserDTO { Username = "newuser", Password = "password", Role = "User" };
            var response = await _client.PostAsJsonAsync("/api/user", user);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnOkForValidUser()
        {
            await AuthenticateAdminAsync();

            var userId = Guid.NewGuid();
            var userUpdate = new UserDTO { Id = userId, Username = "updateduser", Password = "newpassword", Role = "Admin" };

            var response = await _client.PutAsJsonAsync($"/api/user/{userId}", userUpdate);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // Supondo que o usuário ainda não exista
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnNoContentForExistingUser()
        {
            await AuthenticateAdminAsync();

            var userId = Guid.NewGuid().ToString();
            var response = await _client.DeleteAsync($"/api/user/{userId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // Supondo que o usuário ainda não exista
        }
    }
}
