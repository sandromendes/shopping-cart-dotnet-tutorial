﻿using Domain.Models;

namespace Tests.Unit.Domain
{
    public class CartModelUnitTests
    {
        [Fact]
        public void AddItem_ShouldAddNewItemToCart()
        {
            // Arrange
            var cart = new Cart();
            var item = new CartItem(Guid.NewGuid(), "Product A", 2, 10.0m);

            // Act
            cart.AddItem(item);

            // Assert
            Assert.Single(cart.Items);
            Assert.Equal("Product A", cart.Items.First().ProductName);
        }

        [Fact]
        public void AddItem_ShouldIncreaseQuantityIfItemAlreadyExists()
        {
            // Arrange
            var cart = new Cart();
            var cartItemId = Guid.NewGuid();
            var item = new CartItem(cartItemId, "Product A", 2, 10.0m);
            cart.AddItem(item);

            // Act
            cart.AddItem(new CartItem(cartItemId, "Product A", 3, 10.0m));

            // Assert
            Assert.Single(cart.Items);
            Assert.Equal(5, cart.Items.First().Quantity);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItemFromCart()
        {
            // Arrange
            var cart = new Cart();
            var cartItemId = Guid.NewGuid();
            var item = new CartItem(cartItemId, "Product A", 2, 10.0m);
            cart.AddItem(item);

            // Act
            cart.RemoveItem(cartItemId);

            // Assert
            Assert.Empty(cart.Items);
        }

        [Fact]
        public void RemoveItem_ShouldNotRemoveIfItemDoesNotExist()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.RemoveItem(Guid.NewGuid());

            // Assert
            Assert.Empty(cart.Items);
        }

        [Fact]
        public void UpdateItem_ShouldUpdateQuantityAndPriceOfExistingItem()
        {
            // Arrange
            var cart = new Cart();
            var cartItemId = Guid.NewGuid();

            var item = new CartItem(cartItemId, "Product A", 2, 10.0m);
            cart.AddItem(item);

            // Act
            cart.UpdateItem(cartItemId, 5, 15.0m);

            // Assert
            Assert.Equal(5, cart.Items.First().Quantity);
            Assert.Equal(15.0m, cart.Items.First().Price);
        }

        [Fact]
        public void UpdateItem_ShouldNotUpdateIfItemDoesNotExist()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.UpdateItem(Guid.NewGuid(), 5, 15.0m);

            // Assert
            Assert.Empty(cart.Items);
        }
    }

}
