using Domain.Models;

namespace Domain.Infrastructure.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(Guid id);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(Guid id);

        Task<IEnumerable<CartItem>> GetItemsFromCartAsync(Guid cartId);
        Task AddItemToCartAsync(Guid cartId, CartItem item);
        Task UpdateItemInCartAsync(Guid cartId, CartItem item);
        Task RemoveItemFromCartAsync(Guid cartId, Guid itemId);
    }
}
