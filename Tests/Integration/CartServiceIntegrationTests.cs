using AutoMapper;
using Business.Services;
using Domain.Business.Interfaces;
using Domain.Mapping;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Integration
{
    public class CartServiceIntegrationTests : IDisposable
    {
        private readonly ICartService _cartService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CartServiceIntegrationTests()
        {
            // Configuração do banco de dados em memória para testes de integração
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "IntegrationCartDatabase")
                .Options;

            _context = new AppDbContext(dbContextOptions);
            var cartRepository = new CartRepository(_context);

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();

            _cartService = new CartService(cartRepository, _mapper);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddCartAsync_ShouldPersistCart()
        {
            // Arrange
            var cartDto = new CartDTO();

            // Act
            var result = await _cartService.AddCartAsync(cartDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, await _context.Carts.CountAsync());
        }

        [Fact]
        public async Task AddItemToCartAsync_ShouldPersistItem()
        {
            // Arrange
            var cartDto = new CartDTO();
            var cart = await _cartService.AddCartAsync(cartDto);
            var itemDto = new CartItemDTO { ProductId = 1, ProductName = "Product A", Quantity = 2, Price = 10.0m };

            // Act
            var addedItem = await _cartService.AddItemToCartAsync(cart.Id, itemDto);

            // Assert
            Assert.NotNull(addedItem);
            Assert.Single(await _context.CartItems.ToListAsync());
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnCartWithItems()
        {
            // Arrange
            var cartDto = new CartDTO();
            var cart = await _cartService.AddCartAsync(cartDto);
            var itemDto = new CartItemDTO {ProductId = 1, ProductName = "Product A", Quantity = 2, Price = 10.0m };
            await _cartService.AddItemToCartAsync(cart.Id, itemDto);

            // Act
            var result = await _cartService.GetCartAsync(cart.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal("Product A", result.Items.First().ProductName);
        }

        [Fact]
        public async Task UpdateCartAsync_ShouldUpdateCartItems()
        {
            // Arrange
            var cartDto = new CartDTO();
            var cart = await _cartService.AddCartAsync(cartDto);
            var itemDto = new CartItemDTO {ProductId = 1, ProductName = "Product A", Quantity = 2, Price = 10.0m };
            var addedItem = await _cartService.AddItemToCartAsync(cart.Id, itemDto);

            // Update the item
            var updatedItemDto = new CartItemDTO { Id = addedItem.Id, ProductName = "Updated Product A", Quantity = 5, Price = 15.0m };

            // Act
            var updatedItem = await _cartService.UpdateItemInCartAsync(cart.Id, addedItem.Id, updatedItemDto);

            // Assert
            Assert.NotNull(updatedItem);
            Assert.Equal("Updated Product A", updatedItem.ProductName);
            Assert.Equal(5, updatedItem.Quantity);
            Assert.Equal(15.0m, updatedItem.Price);
        }

        [Fact]
        public async Task DeleteCartAsync_ShouldRemoveCart()
        {
            // Arrange
            var cartDto = new CartDTO();
            var cart = await _cartService.AddCartAsync(cartDto);

            // Act
            var result = await _cartService.DeleteCartAsync(cart.Id);

            // Assert
            Assert.True(result);
            Assert.Empty(await _context.Carts.ToListAsync());
        }

        [Fact]
        public async Task DeleteItemFromCartAsync_ShouldRemoveItem()
        {
            // Arrange
            var cartDto = new CartDTO();
            var cart = await _cartService.AddCartAsync(cartDto);
            var itemDto = new CartItemDTO { ProductId = 1, ProductName = "Product A", Quantity = 2, Price = 10.0m };
            var addedItem = await _cartService.AddItemToCartAsync(cart.Id, itemDto);

            // Act
            var result = await _cartService.RemoveItemFromCartAsync(cart.Id, addedItem.Id);

            // Assert
            Assert.True(result);
            Assert.Empty(await _context.CartItems.ToListAsync());
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnNullIfCartDoesNotExist()
        {
            // Act
            var result = await _cartService.GetCartAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCartAsync_ShouldThrowIfCartDtoIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _cartService.UpdateCartAsync(null));
        }

        [Fact]
        public async Task UpdateItemInCartAsync_ShouldReturnNullIfItemDoesNotExist()
        {
            // Arrange
            var cartDto = new CartDTO();
            var cart = await _cartService.AddCartAsync(cartDto);
            var itemDto = new CartItemDTO {ProductId = 1, ProductName = "Product A", Quantity = 2, Price = 10.0m };
            await _cartService.AddItemToCartAsync(cart.Id, itemDto);

            // Act
            var result = await _cartService.UpdateItemInCartAsync(cart.Id, 999, itemDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_ShouldReturnFalseIfItemDoesNotExist()
        {
            // Arrange
            var cartDto = new CartDTO();
            var cart = await _cartService.AddCartAsync(cartDto);
            var itemDto = new CartItemDTO {ProductId = 1, ProductName = "Product A", Quantity = 2, Price = 10.0m };
            await _cartService.AddItemToCartAsync(cart.Id, itemDto);

            // Act
            var result = await _cartService.RemoveItemFromCartAsync(cart.Id, 999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CartItem_ShouldThrowExceptionIfInvalidDataOnCreation()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CartItem(0, string.Empty, 1, 10.0m)); // Invalid product name
            Assert.Throws<ArgumentException>(() => new CartItem(1, "Product A", 0, 10.0m)); // Invalid quantity
            Assert.Throws<ArgumentException>(() => new CartItem(1, "Product A", 1, -5.0m)); // Invalid price
        }

        [Fact]
        public void CartItem_UpdateQuantity_ShouldThrowExceptionIfInvalidQuantity()
        {
            // Arrange
            var item = new CartItem(1, "Product A", 1, 10.0m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => item.UpdateQuantity(0)); // Invalid quantity
        }

        [Fact]
        public void CartItem_UpdatePrice_ShouldThrowExceptionIfInvalidPrice()
        {
            // Arrange
            var item = new CartItem(1, "Product A", 1, 10.0m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => item.UpdatePrice(-5.0m)); // Invalid price
        }

        [Fact]
        public void CartItem_UpdateFrom_ShouldThrowExceptionIfUpdatedItemIsNull()
        {
            // Arrange
            var item = new CartItem(1, "Product A", 1, 10.0m);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => item.UpdateFrom(null)); // Null item
        }
    }
}
