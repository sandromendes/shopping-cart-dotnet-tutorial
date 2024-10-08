using Domain.Mapping;

namespace Domain.Business.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO> GetCartAsync(int id);
        Task<CartDTO> AddCartAsync(CartDTO cartDto);
        Task<CartDTO> UpdateCartAsync(CartDTO cartDto);
        Task<bool> DeleteCartAsync(int id);
        Task<IEnumerable<CartItemDTO>> GetItemsFromCartAsync(int cartId);
        Task<CartItemDTO> AddItemToCartAsync(int cartId, CartItemDTO itemDto);
        Task<bool> RemoveItemFromCartAsync(int cartId, int itemId);
        Task<CartItemDTO> UpdateItemInCartAsync(int cartId, int itemId, CartItemDTO itemDto);
    }
}
