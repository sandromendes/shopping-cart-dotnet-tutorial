using System.ComponentModel.DataAnnotations;

namespace Domain.Mapping
{
    public class CartItemDTO
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int CartId { get; set; }

        public CartItemDTO()
        {
        }

        public CartItemDTO(int productId, string productName, int quantity, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}
