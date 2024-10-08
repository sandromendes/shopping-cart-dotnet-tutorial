using Domain.Business.Interfaces;
using Domain.Mapping;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartAsync(int id)
        {
            var cart = await _cartService.GetCartAsync(id);

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

            return CreatedAtAction(nameof(GetCartAsync), new { id = createdCart.Id }, createdCart);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartAsync(int id, [FromBody] CartDTO cartDto)
        {
            if (id != cartDto.Id)
                return BadRequest();

            var updatedCart = await _cartService.UpdateCartAsync(cartDto);
            if (updatedCart == null)
                return NotFound();

            return Ok(updatedCart);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartAsync(int id)
        {
            if(id < 0)
                return BadRequest();

            var deleted = await _cartService.DeleteCartAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{cartId}/items")]
        public async Task<IActionResult> GetItemsFromCartAsync(int cartId)
        {
            var items = await _cartService.GetItemsFromCartAsync(cartId);
            if (items == null)
                return NotFound();

            return Ok(items);
        }

        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> AddItemToCartAsync(int cartId, [FromBody] CartItemDTO itemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _cartService.AddItemToCartAsync(cartId, itemDto);
            return CreatedAtAction(nameof(GetItemsFromCartAsync), new { cartId = cartId }, item);
        }

        [HttpDelete("{cartId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCartAsync(int cartId, int itemId)
        {
            var removed = await _cartService.RemoveItemFromCartAsync(cartId, itemId);
            if (!removed)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{cartId}/items/{itemId}")]
        public async Task<IActionResult> UpdateItemInCartAsync(int cartId, int itemId, [FromBody] CartItemDTO itemDto)
        {
            if (!ModelState.IsValid || cartId != itemDto.CartId || itemId != itemDto.Id)
                return BadRequest();

            var updatedItem = await _cartService.UpdateItemInCartAsync(cartId, itemId, itemDto);
            if (updatedItem == null)
                return NotFound();

            return Ok(updatedItem);
        }
    }
}
