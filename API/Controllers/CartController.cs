using Business.Commands;
using Business.Pagination;
using Business.Queries;
using Domain.Transfer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartAsync(string cartId)
        {
            if (!Guid.TryParse(cartId, out var parsedCartId))
                return BadRequest();

            var cart = await _mediator.Send(new GetCartQuery(parsedCartId));

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddCartAsync([FromBody] CartDTO cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCart = await _mediator.Send(new AddCartCommand(cartDto));

            return CreatedAtAction(nameof(GetCartAsync), new { cartId = createdCart.Id }, createdCart);
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCartAsync(string cartId)
        {
            if (!Guid.TryParse(cartId, out var parsedCartId))
                return BadRequest();

            var deleted = await _mediator.Send(new DeleteCartCommand(parsedCartId));

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{cartId}/items")]
        public async Task<IActionResult> GetItemsFromCartAsync(string cartId,
            [FromQuery] PageInfo pagination,
            [FromQuery] Ordering ordering,
            [FromQuery] Filter filter)
        {
            if (!Guid.TryParse(cartId, out var parsedCartId))
                return BadRequest();

            var items = await _mediator.Send(new GetItemsFromCartQuery(parsedCartId, pagination, ordering, filter));

            if (items == null)
                return NotFound();

            return Ok(items);
        }

        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> AddItemToCartAsync(string cartId, 
            [FromBody] CartItemDTO itemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!Guid.TryParse(cartId, out var parsedCartId))
                return BadRequest();

            var item = await _mediator.Send(new AddItemToCartCommand(parsedCartId, itemDto));

            return CreatedAtAction(nameof(GetItemsFromCartAsync), new { cartId }, item);
        }

        [HttpDelete("{cartId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCartAsync(string cartId, string itemId)
        {
            if (!Guid.TryParse(cartId, out var parsedCartId) 
                || !Guid.TryParse(itemId, out var parsedItemId))
                return BadRequest();

            var removed = await _mediator.Send(new RemoveItemFromCartCommand(parsedCartId, parsedItemId));

            if (!removed)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{cartId}/items/{itemId}")]
        public async Task<IActionResult> UpdateItemInCartAsync(string cartId, string itemId, 
            [FromBody] CartItemDTO itemDto)
        {
            if (!Guid.TryParse(cartId, out var parsedCartId) || !Guid.TryParse(itemId, out var parsedItemId))
                return BadRequest();

            if (!ModelState.IsValid || parsedCartId != itemDto.CartId || parsedItemId != itemDto.Id)
                return BadRequest();

            var updatedItem = await _mediator.Send(new UpdateItemInCartCommand(parsedCartId, parsedItemId, itemDto));

            if (updatedItem == null)
                return NotFound();

            return Ok(updatedItem);
        }
    }
}
