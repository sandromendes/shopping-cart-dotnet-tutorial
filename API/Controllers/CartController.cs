using Domain.Business.Interfaces;
using Domain.Transfer;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartAsync(string cartId)
        {
            if (!Guid.TryParse(cartId, out _))
                return BadRequest();

            var cart = await _cartService.GetCartAsync(Guid.Parse(cartId));

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddCartAsync([FromBody] CartDTO cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCart = await _cartService.AddCartAsync(cartDto);

            return CreatedAtAction(nameof(GetCartAsync), new { cartId = createdCart.Id }, createdCart);
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCartAsync(string cartId)
        {
            if(!Guid.TryParse(cartId, out _))
                return BadRequest();

            var deleted = await _cartService.DeleteCartAsync(Guid.Parse(cartId));

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{cartId}/items")]
        public async Task<IActionResult> GetItemsFromCartAsync(string cartId)
        {
            if (!Guid.TryParse(cartId, out _))
                return BadRequest();

            var items = await _cartService.GetItemsFromCartAsync(Guid.Parse(cartId));
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

            if (!Guid.TryParse(cartId, out _))
                return BadRequest();

            var item = await _cartService.AddItemToCartAsync(Guid.Parse(cartId), itemDto);
            return CreatedAtAction(nameof(GetItemsFromCartAsync), new { cartId }, item);
        }

        [HttpDelete("{cartId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCartAsync(string cartId, string itemId)
        {
            if (!Guid.TryParse(cartId, out _))
                return BadRequest();

            if (!Guid.TryParse(itemId, out _))
                return BadRequest();

            var removed = await _cartService
                .RemoveItemFromCartAsync(Guid.Parse(cartId), Guid.Parse(itemId));
            
            if (!removed)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{cartId}/items/{itemId}")]
        public async Task<IActionResult> UpdateItemInCartAsync(string cartId, string itemId, 
            [FromBody] CartItemDTO itemDto)
        {
            if (!Guid.TryParse(cartId, out _))
                return BadRequest();

            if (!Guid.TryParse(itemId, out _))
                return BadRequest();

            if (!ModelState.IsValid 
                || Guid.Parse(cartId) != itemDto.CartId 
                || Guid.Parse(itemId) != itemDto.Id)
                return BadRequest();

            var updatedItem = await _cartService
                .UpdateItemInCartAsync(Guid.Parse(cartId), Guid.Parse(itemId), itemDto);

            if (updatedItem == null)
                return NotFound();

            return Ok(updatedItem);
        }
    }
}
