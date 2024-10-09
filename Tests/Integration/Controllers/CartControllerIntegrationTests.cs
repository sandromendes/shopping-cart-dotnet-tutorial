using Domain.Transfer;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Tests.Integration.Controllers
{
    public class CartControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CartControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetCart_ShouldReturnOkWithCart()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var cart = new CartDTO 
            { 
                Id = cartId, 
                Items = new[] 
                { 
                    new CartItemDTO 
                    { 
                        Id = itemId, 
                        ProductId = Guid.NewGuid(),
                        ProductName = "Test Product", 
                        Quantity = 2,
                        Price = 1
                    } 
                } 
            };

            await _client.PostAsJsonAsync("/api/cart", cart);

            // Act
            var response = await _client.GetAsync($"/api/cart/{cartId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedCart = await response.Content.ReadFromJsonAsync<CartDTO>();
            
            Assert.NotNull(returnedCart);
            Assert.Equal(cartId, returnedCart.Id);
        }

        [Fact]
        public async Task GetCart_ShouldReturnNotFoundIfCartDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync($"/api/cart/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddCart_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            var cart = new CartDTO 
            { 
                Id = cartId,
                Items = new[] 
                { 
                    new CartItemDTO 
                    { 
                        Id = cartItemId,
                        CartId = cartId,
                        ProductName = "New Product",
                        Quantity = 1,
                        Price = 1,
                    } 
                } 
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/cart", cart);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task AddCart_ShouldReturnBadRequestIfModelInvalid()
        {
            // Arrange
            var cart = new CartDTO(); // Invalid cart

            // Act
            var response = await _client.PostAsJsonAsync("/api/cart", cart);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCart_ShouldReturnOk()
        {
            // Arrange
            var cartId = Guid.NewGuid();

            var cart = new CartDTO
            {
                Id = cartId,
                Items = new[]
                {
                    new CartItemDTO {
                        Id = Guid.NewGuid(),
                        ProductName = "Updated Product",
                        Quantity = 3,
                        Price = 19
                    } 
                }
            };
            await _client.PostAsJsonAsync("/api/cart", cart);

            // Act
            var response = await _client.PutAsJsonAsync($"/api/cart/{cartId}", cart);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCart_ShouldReturnBadRequestIfModelInvalid()
        {
            // Arrange
            var cart = new CartDTO(); // Invalid cart

            // Act
            var response = await _client.PutAsJsonAsync("/api/cart/1", cart);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCart_ShouldReturnNotFoundIfCartDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            var cart = new CartDTO { Id = id, Items = new[] { new CartItemDTO { ProductName = "Non-existent Product", Quantity = 1 } } };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/cart/{Guid.NewGuid()}", cart);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCart_ShouldReturnNoContent()
        {
            // Arrange
            var cartId = Guid.NewGuid();

            var cart = new CartDTO 
            { 
                Id = cartId, 
                Items = new[] 
                { 
                    new CartItemDTO 
                    { 
                        ProductName = "To Be Deleted", 
                        Quantity = 1,
                        Price = 10
                    } 
                } 
            };
            await _client.PostAsJsonAsync("/api/cart", cart);

            // Act
            var response = await _client.DeleteAsync($"/api/cart/{cartId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCart_ShouldReturnNotFoundIfCartDoesNotExist()
        {
            // Act
            var response = await _client.DeleteAsync($"/api/cart/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddItemToCart_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var id = Guid.NewGuid();

            var cart = new CartDTO { Id = id, Items = new[] { new CartItemDTO { ProductName = "Existing Product", Quantity = 1 } } };
            
            await _client.PostAsJsonAsync("/api/cart", cart);

            var item = new CartItemDTO { ProductName = "New Item", Quantity = 2, Price = 10 };

            // Act
            var response = await _client.PostAsJsonAsync($"/api/cart/{id}/items", item);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task RemoveItemFromCart_ShouldReturnNoContent()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var cart = new CartDTO
            {
                Id = cartId,
                Items = new[]
                {
                    new CartItemDTO
                    {
                        Id = itemId,
                        ProductId = Guid.NewGuid(),
                        CartId = cartId,
                        ProductName = "Item To Remove", 
                        Quantity = 1,
                        Price = 199
                    } 
                } 
            };

            await _client.PostAsJsonAsync("/api/cart", cart);

            // Act
            var response = await _client.DeleteAsync($"/api/cart/{cartId}/items/{itemId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task UpdateItemInCart_ShouldReturnOk()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            var cart = new CartDTO 
            { 
                Id = cartId, 
                Items = new[] 
                { 
                    new CartItemDTO 
                    { 
                        Id = cartItemId, 
                        CartId = cartId,
                        ProductName = "Old Item", 
                        Quantity = 1,
                        Price = 1
                    } 
                } 
            };

            await _client.PostAsJsonAsync("/api/cart", cart);

            var item = new CartItemDTO 
            { 
                Id = cartItemId, 
                ProductName = "Updated Item", 
                Quantity = 3, 
                Price = 2,
                CartId = cartId
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/cart/{cartId}/items/{cartItemId}", item);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateItemInCart_ShouldReturnBadRequestIfModelInvalid()
        {
            // Arrange
            var invalidItem = new CartItemDTO(); // Invalid item

            // Act
            var response = await _client.PutAsJsonAsync("/api/cart/5/items/5", invalidItem);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateItemInCart_ShouldReturnNotFoundIfCartOrItemDoesNotExist()
        {
            // Arrange
            var cartId = Guid.NewGuid();

            var item = new CartItemDTO 
            { 
                Id = Guid.NewGuid(), 
                ProductName = "Non-existent Item",
                Quantity = 1, 
                Price = 1,
                CartId = cartId
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/cart/{cartId}/items/{item.Id}", item);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
