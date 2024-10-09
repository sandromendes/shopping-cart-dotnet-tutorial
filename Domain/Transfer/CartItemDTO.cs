using System.ComponentModel.DataAnnotations;

namespace Domain.Transfer
{
    public class CartItemDTO
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public Guid CartId { get; set; }

        public CartItemDTO()
        {
        }

        public CartItemDTO(Guid productId, string productName, int quantity, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}
