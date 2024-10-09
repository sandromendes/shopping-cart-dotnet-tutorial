namespace Domain.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        private List<CartItem> _items = new List<CartItem>();

        public IList<CartItem> Items => _items;
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

        public void RemoveItem(Guid productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public void UpdateItem(Guid productId, int quantity, decimal price)
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
