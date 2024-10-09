using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Unit.Repositories
{
    public class CartRepositoryUnitTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CartDatabase")
                .Options;
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnCart_WhenCartExists()
        {
            // Arrange
            var options = GetDbContextOptions();
            var cart = new Cart();
            using (var context = new AppDbContext(options))
            {
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new CartRepository(context);

                // Act
                var result = await repository.GetCartAsync(cart.Id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(cart.Id, result.Id);
            }
        }

        [Fact]
        public async Task AddCartAsync_ShouldAddCartToDatabase()
        {
            // Arrange
            var options = GetDbContextOptions();
            var cart = new Cart();

            using (var context = new AppDbContext(options))
            {
                var repository = new CartRepository(context);

                // Act
                await repository.AddCartAsync(cart);

                // Assert
                var addedCart = await context.Carts.FindAsync(cart.Id);
                Assert.NotNull(addedCart);
                Assert.Equal(cart.Id, addedCart.Id);
            }
        }

        [Fact]
        public async Task UpdateCartAsync_ShouldUpdateCartInDatabase()
        {
            // Arrange
            var options = GetDbContextOptions();
            var cart = new Cart();

            using (var context = new AppDbContext(options))
            {
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new CartRepository(context);

                var productId = Guid.NewGuid();
                cart.AddItem(new CartItem(productId, "Product A", 1, 10m));

                // Act
                await repository.UpdateCartAsync(cart);

                // Assert
                var updatedCart = await context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cart.Id);
                Assert.Single(updatedCart.Items);
                Assert.Equal("Product A", updatedCart.Items[0].ProductName);
            }
        }

        [Fact]
        public async Task DeleteCartAsync_ShouldRemoveCartFromDatabase()
        {
            // Arrange
            var options = GetDbContextOptions();
            var cart = new Cart();

            using (var context = new AppDbContext(options))
            {
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new CartRepository(context);

                // Act
                await repository.DeleteCartAsync(cart.Id);

                // Assert
                var deletedCart = await context.Carts.FindAsync(cart.Id);
                Assert.Null(deletedCart);
            }
        }

        [Fact]
        public async Task AddItemToCartAsync_ShouldAddItemToCart()
        {
            // Arrange
            var options = GetDbContextOptions();
            var cart = new Cart();

            using (var context = new AppDbContext(options))
            {
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new CartRepository(context);
                var productId = Guid.NewGuid();
                var cartItem = new CartItem(productId, "Product B", 2, 20m);

                // Act
                await repository.AddItemToCartAsync(cart.Id, cartItem);

                // Assert
                var updatedCart = await context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cart.Id);
                Assert.Single(updatedCart.Items);
                Assert.Equal("Product B", updatedCart.Items[0].ProductName);
            }
        }

        [Fact]
        public async Task UpdateItemInCartAsync_ShouldUpdateItemInCart()
        {
            // Arrange
            var options = GetDbContextOptions();
            var cart = new Cart();
            var productId = Guid.NewGuid();
            var cartItem = new CartItem(productId, "Product C", 1, 30m);

            using (var context = new AppDbContext(options))
            {
                cart.AddItem(cartItem);
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new CartRepository(context);
                var updatedItem = new CartItem(productId, "Product C", 3, 35m) { Id = cartItem.Id };

                // Act
                await repository.UpdateItemInCartAsync(cart.Id, updatedItem);

                // Assert
                var updatedCart = await context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cart.Id);
                var item = updatedCart.Items.First();
                Assert.Equal(3, item.Quantity);
                Assert.Equal(35m, item.Price);
            }
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_ShouldRemoveItemFromCart()
        {
            // Arrange
            var options = GetDbContextOptions();
            var cart = new Cart();
            var cartItemId = Guid.NewGuid();
            var cartItem = new CartItem(cartItemId, "Product D", 1, 40m);

            using (var context = new AppDbContext(options))
            {
                cart.AddItem(cartItem);
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new CartRepository(context);

                // Act
                await repository.RemoveItemFromCartAsync(cart.Id, cartItem.Id);

                // Assert
                var updatedCart = await context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cart.Id);
                Assert.Empty(updatedCart.Items);
            }
        }

        [Fact]
        public async Task GetItemsFromCartAsync_ShouldReturnItems_WhenItemsExist()
        {
            // Arrange
            var options = GetDbContextOptions();
            var cart = new Cart();
            var cartItemId1 = Guid.NewGuid();
            var cartItem1 = new CartItem(cartItemId1, "Product E", 1, 50m);
            var cartItemId2 = Guid.NewGuid();
            var cartItem2 = new CartItem(cartItemId2, "Product F", 2, 60m);

            using (var context = new AppDbContext(options))
            {
                cart.AddItem(cartItem1);
                cart.AddItem(cartItem2);
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new CartRepository(context);

                // Act
                var items = await repository.GetItemsFromCartAsync(cart.Id);

                // Assert
                Assert.Equal(2, items.Count());
                Assert.Contains(items, i => i.ProductName == "Product E");
                Assert.Contains(items, i => i.ProductName == "Product F");
            }
        }
    }
}
