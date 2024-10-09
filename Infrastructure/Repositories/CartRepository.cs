using Domain.Infrastructure.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(Guid id)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCartAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(Guid id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CartItem>> GetItemsFromCartAsync(Guid cartId)
        {
            var cart = await GetCartAsync(cartId);
            return cart?.Items ?? new List<CartItem>();
        }

        public async Task AddItemToCartAsync(Guid cartId, CartItem item)
        {
            var cart = await GetCartAsync(cartId);
            if (cart != null)
            {
                cart.AddItem(item);
                await UpdateCartAsync(cart);
            }
        }

        public async Task UpdateItemInCartAsync(Guid cartId, CartItem item)
        {
            var cart = await GetCartAsync(cartId);
            if (cart != null)
            {
                var existingItem = cart.Items.FirstOrDefault(i => i.Id == item.Id);
                if (existingItem != null)
                {
                    existingItem.UpdateFrom(item);
                    await UpdateCartAsync(cart);
                }
            }
        }

        public async Task RemoveItemFromCartAsync(Guid cartId, Guid itemId)
        {
            var cart = await GetCartAsync(cartId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    cart.RemoveItem(item.ProductId);
                    await UpdateCartAsync(cart);
                }
            }
        }
    }
}
