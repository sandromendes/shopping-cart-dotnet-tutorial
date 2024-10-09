namespace Domain.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal Subtotal => Quantity * Price;

        public Guid CartId { get; set; }
        public Cart Cart { get; set; }

        public CartItem(Guid productId, string productName, int quantity, decimal price)
        {
            if (!Guid.TryParse(productId.ToString(), out _)) throw new ArgumentException(null, nameof(productId));

            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name cannot be empty.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            Quantity = quantity;
        }

        public void UpdatePrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.");
            Price = price;
        }

        public void UpdateFrom(CartItem updatedItem)
        {
            if (updatedItem == null)
                throw new ArgumentNullException(nameof(updatedItem));

            ProductId = updatedItem.ProductId;
            ProductName = updatedItem.ProductName;
            Price = updatedItem.Price;
            Quantity = updatedItem.Quantity;
        }
    }
}
