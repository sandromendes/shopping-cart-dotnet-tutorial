using AutoMapper;
using Business.Services;
using Domain.Business.Interfaces;
using Domain.Infrastructure.Interfaces;
using Domain.Mapping;
using Domain.Models;
using Moq;

namespace Tests.Unit.Services
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICartService _cartService;

        public CartServiceTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _mapperMock = new Mock<IMapper>();
            _cartService = new CartService(_cartRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnCartDto()
        {
            // Arrange
            var cart = new Cart { Id = 1 };
            var cartDto = new CartDTO { Id = 1 };

            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(1)).ReturnsAsync(cart);
            _mapperMock.Setup(m => m.Map<CartDTO>(cart)).Returns(cartDto);

            // Act
            var result = await _cartService.GetCartAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _cartRepositoryMock.Verify(repo => repo.GetCartAsync(1), Times.Once);
        }

        [Fact]
        public async Task AddCartAsync_ShouldCallRepositoryAndReturnCartDto()
        {
            // Arrange
            var cartDto = new CartDTO();
            var cart = new Cart { Id = 1 };

            _mapperMock.Setup(m => m.Map<Cart>(cartDto)).Returns(cart);
            _mapperMock.Setup(m => m.Map<CartDTO>(cart)).Returns(cartDto);

            // Act
            var result = await _cartService.AddCartAsync(cartDto);

            // Assert
            _cartRepositoryMock.Verify(repo => repo.AddCartAsync(cart), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(cartDto, result);
        }

        [Fact]
        public async Task UpdateCartAsync_ShouldUpdateCartAndReturnCartDto()
        {
            // Arrange
            var cartDto = new CartDTO { Id = 1 };
            var cart = new Cart { Id = 1 };

            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(1)).ReturnsAsync(cart);
            _mapperMock.Setup(m => m.Map<Cart>(cartDto)).Returns(cart);
            _mapperMock.Setup(m => m.Map<CartDTO>(cart)).Returns(cartDto);

            // Act
            var result = await _cartService.UpdateCartAsync(cartDto);

            // Assert
            _cartRepositoryMock.Verify(repo => repo.UpdateCartAsync(cart), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(cartDto.Id, result.Id);
        }

        [Fact]
        public async Task DeleteCartAsync_ShouldCallRepositoryAndReturnTrue()
        {
            // Arrange
            var cartId = 1;
            var cart = new Cart { Id = cartId };

            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(cartId)).ReturnsAsync(cart);

            // Act
            var result = await _cartService.DeleteCartAsync(cartId);

            // Assert
            Assert.True(result);
            _cartRepositoryMock.Verify(repo => repo.DeleteCartAsync(cartId), Times.Once);
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnNullIfCartDoesNotExist()
        {
            // Arrange
            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(It.IsAny<int>())).ReturnsAsync((Cart)null);

            // Act
            var result = await _cartService.GetCartAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddCartAsync_ShouldThrowIfCartDtoIsNull()
        {
            // Arrange
            CartDTO cartDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _cartService.AddCartAsync(cartDto));
        }

        [Fact]
        public async Task UpdateCartAsync_ShouldReturnNullIfCartDoesNotExist()
        {
            // Arrange
            var cartDto = new CartDTO { Id = 1 };
            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(1)).ReturnsAsync((Cart)null);

            // Act
            var result = await _cartService.UpdateCartAsync(cartDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_ShouldReturnFalseIfCartDoesNotExist()
        {
            // Arrange
            var cartId = 1;
            var itemId = 1;
            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(cartId)).ReturnsAsync((Cart)null);

            // Act
            var result = await _cartService.RemoveItemFromCartAsync(cartId, itemId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddItemToCartAsync_ShouldAddItemAndReturnItemDto()
        {
            // Arrange
            var cartId = 1;
            var itemDto = new CartItemDTO { Id = 1 };
            var cart = new Cart { Id = cartId };
            var item = new CartItem(1, "Product", 1, 10.0m) { Id = itemDto.Id };

            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(cartId)).ReturnsAsync(cart);
            _mapperMock.Setup(m => m.Map<CartItem>(itemDto)).Returns(item);
            _mapperMock.Setup(m => m.Map<CartItemDTO>(item)).Returns(itemDto);

            // Act
            var result = await _cartService.AddItemToCartAsync(cartId, itemDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(itemDto.Id, result.Id);
            _cartRepositoryMock.Verify(repo => repo.UpdateCartAsync(cart), Times.Once);
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_ShouldReturnFalseIfItemDoesNotExist()
        {
            // Arrange
            var cartId = 1;
            var itemId = 999; // Non-existing item ID
            var cart = new Cart { Id = cartId };

            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(cartId)).ReturnsAsync(cart);

            // Act
            var result = await _cartService.RemoveItemFromCartAsync(cartId, itemId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateItemInCartAsync_ShouldReturnNullIfCartDoesNotExist()
        {
            // Arrange
            var cartId = 1;
            var itemDto = new CartItemDTO { Id = 1 };
            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(cartId)).ReturnsAsync((Cart)null);

            // Act
            var result = await _cartService.UpdateItemInCartAsync(cartId, itemDto.Id, itemDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateItemInCartAsync_ShouldReturnNullIfItemDoesNotExist()
        {
            // Arrange
            var cartId = 1;
            var itemDto = new CartItemDTO { Id = 1 };
            var cart = new Cart { Id = cartId };

            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(cartId)).ReturnsAsync(cart);

            // Act
            var result = await _cartService.UpdateItemInCartAsync(cartId, itemDto.Id, itemDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateItemInCartAsync_ShouldUpdateItemAndReturnItemDto()
        {
            // Arrange
            var cartId = 1;
            var itemId = 1;
            var itemDto = new CartItemDTO { Id = itemId };
            var cart = new Cart { Id = cartId };
            var existingItem = new CartItem(1, "Existing Product", 1, 10.0m) { Id = itemId };

            cart.AddItem(existingItem);
            _cartRepositoryMock.Setup(repo => repo.GetCartAsync(cartId)).ReturnsAsync(cart);
            _mapperMock.Setup(m => m.Map<CartItem>(itemDto)).Returns(existingItem);
            _mapperMock.Setup(m => m.Map<CartItemDTO>(existingItem)).Returns(itemDto);

            // Act
            var result = await _cartService.UpdateItemInCartAsync(cartId, itemId, itemDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(itemId, result.Id);
            _cartRepositoryMock.Verify(repo => repo.UpdateCartAsync(cart), Times.Once);
        }
    }
}
