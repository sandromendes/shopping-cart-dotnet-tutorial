namespace Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        private readonly List<CartItem> _items = new List<CartItem>();

        public IList<CartItem> Items => _items.AsReadOnly();
        public decimal Total => _items.Sum(item => item.Subtotal);

        public void AddItem(CartItem item)
        {
            var existingItem = _items.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
            }
            else
            {
                _items.Add(item);
            }
        }

        public void RemoveItem(int  productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public void UpdateItem(int productId, int quantity, decimal price)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.UpdateQuantity(quantity);
                item.UpdatePrice(price);
            }
        }
    }
}
