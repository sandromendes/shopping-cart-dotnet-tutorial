using API.Controllers;
using Domain.Business.Interfaces;
using Domain.Transfer;
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
            _cartServiceMock.Setup(service => service.GetCartAsync(It.IsAny<Guid>())).ReturnsAsync(cartDto);

            // Act
            var result = await _cartController.GetCartAsync(Guid.NewGuid().ToString());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cartDto, okResult.Value);
        }

        [Fact]
        public async Task GetCart_ShouldReturnNotFoundIfCartDoesNotExist()
        {
            // Arrange
            _cartServiceMock.Setup(service => service.GetCartAsync(It.IsAny<Guid>())).ReturnsAsync((CartDTO)null);

            // Act
            var result = await _cartController.GetCartAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddCart_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var cartDto = new CartDTO { Id = Guid.NewGuid() };
            _cartServiceMock.Setup(service => service.AddCartAsync(cartDto)).ReturnsAsync(cartDto);

            // Act
            var result = await _cartController.AddCartAsync(cartDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetCartAsync", createdAtActionResult.ActionName);
            Assert.Equal(cartDto.Id, createdAtActionResult.RouteValues["cartId"]);
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
        public async Task DeleteCart_ShouldReturnNoContent()
        {
            // Arrange
            _cartServiceMock.Setup(service => service.DeleteCartAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            // Act
            var result = await _cartController.DeleteCartAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCart_ShouldReturnNotFoundIfCartDoesNotExist()
        {
            // Arrange
            _cartServiceMock.Setup(service => service.DeleteCartAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            var result = await _cartController.DeleteCartAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCart_ShouldReturnBadRequestIfInvalidId()
        {
            // Arrange
            _cartController.ModelState.AddModelError("Error", "Invalid Id");

            // Act
            var result = await _cartController.DeleteCartAsync("Invalid Id");

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }

}
