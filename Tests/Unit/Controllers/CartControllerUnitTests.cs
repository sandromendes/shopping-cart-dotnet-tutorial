using API.Controllers;
using Business.Commands;
using Business.Queries;
using Domain.Transfer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Unit.Controllers
{
    public class CartControllerUnitTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CartController _cartController;

        public CartControllerUnitTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _cartController = new CartController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetCart_ShouldReturnOkWithCart()
        {
            // Arrange
            var cartDto = new CartDTO();
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartDto);

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
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CartDTO)null);

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
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<AddCartCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartDto);

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
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<DeleteCartCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _cartController.DeleteCartAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCart_ShouldReturnNotFoundIfCartDoesNotExist()
        {
            // Arrange
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<DeleteCartCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

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
