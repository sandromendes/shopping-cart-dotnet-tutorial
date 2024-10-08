using Domain.Models;

namespace Domain.Infrastructure.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(int id);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(int id);

        Task<IEnumerable<CartItem>> GetItemsFromCartAsync(int cartId);
        Task AddItemToCartAsync(int cartId, CartItem item);
        Task UpdateItemInCartAsync(int cartId, CartItem item);
        Task RemoveItemFromCartAsync(int cartId, int itemId);
    }
}
