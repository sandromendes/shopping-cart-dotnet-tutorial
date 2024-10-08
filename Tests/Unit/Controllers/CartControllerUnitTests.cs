using API.Controllers;
using Domain.Business.Interfaces;
using Domain.Mapping;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Unit.Controllers
{
    public class CartControllerUnitTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly CartController _cartController;

        public CartControllerUnitTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _cartController = new CartController(_cartServiceMock.Object);
        }

        [Fact]
        public async Task GetCart_ShouldReturnOkWithCart()
        {
            // Arrange
            var cartDto = new CartDTO();
            _cartServiceMock.Setup(service => service.GetCartAsync(It.IsAny<int>())).ReturnsAsync(cartDto);

            // Act
            var result = await _cartController.GetCartAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cartDto, okResult.Value);
        }

        [Fact]
        public async Task GetCart_ShouldReturnNotFoundIfCartDoesNotExist()
        {
            // Arrange
            _cartServiceMock.Setup(service => service.GetCartAsync(It.IsAny<int>())).ReturnsAsync((CartDTO)null);

            // Act
            var result = await _cartController.GetCartAsync(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddCart_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var cartDto = new CartDTO { Id = 1 }; // Assumindo que o ID é gerado no serviço
            _cartServiceMock.Setup(service => service.AddCartAsync(cartDto)).ReturnsAsync(cartDto);

            // Act
            var result = await _cartController.AddCartAsync(cartDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetCartAsync", createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task AddCart_ShouldReturnBadRequestIfModelInvalid()
        {
            // Arrange
            _cartController.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _cartController.AddCartAsync(new CartDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCart_ShouldReturnNoContent()
        {
            // Arrange
            var cartDto = new CartDTO { Id = 1 };
            _cartServiceMock.Setup(service => service.UpdateCartAsync(cartDto)).ReturnsAsync(cartDto);

            // Act
            var result = await _cartController.UpdateCartAsync(cartDto.Id, cartDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCart_ShouldReturnBadRequestIfModelInvalid()
        {
            // Arrange
            _cartController.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _cartController.UpdateCartAsync(1, new CartDTO());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateCart_ShouldReturnNotFoundIfCartDoesNotExist()
        {
            // Arrange
            var cartDto = new CartDTO { Id = 999 };
            _cartServiceMock.Setup(service => service.UpdateCartAsync(cartDto)).ReturnsAsync((CartDTO)null);

            // Act
            var result = await _cartController.UpdateCartAsync(cartDto.Id, cartDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCart_ShouldReturnNoContent()
        {
            // Arrange
            _cartServiceMock.Setup(service => service.DeleteCartAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _cartController.DeleteCartAsync(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCart_ShouldReturnNotFoundIfCartDoesNotExist()
        {
            // Arrange
            _cartServiceMock.Setup(service => service.DeleteCartAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _cartController.DeleteCartAsync(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCart_ShouldReturnBadRequestIfInvalidId()
        {
            // Arrange
            _cartController.ModelState.AddModelError("Error", "Invalid Id");

            // Act
            var result = await _cartController.DeleteCartAsync(-1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }

}
