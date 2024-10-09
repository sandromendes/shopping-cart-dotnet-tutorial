using Domain.Transfer;

namespace Domain.Business.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO> GetCartAsync(Guid id);
        Task<CartDTO> AddCartAsync(CartDTO cartDto);
        Task<CartDTO> UpdateCartAsync(CartDTO cartDto);
        Task<bool> DeleteCartAsync(Guid id);
        Task<IEnumerable<CartItemDTO>> GetItemsFromCartAsync(Guid cartId);
        Task<CartItemDTO> AddItemToCartAsync(Guid cartId, CartItemDTO itemDto);
        Task<bool> RemoveItemFromCartAsync(Guid cartId, Guid itemId);
        Task<CartItemDTO> UpdateItemInCartAsync(Guid cartId, Guid itemId, CartItemDTO itemDto);
    }
}
