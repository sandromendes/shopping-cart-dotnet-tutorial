namespace Domain.Transfer
{
    public class CartDTO
    {
        public Guid Id { get; set; }

        public ICollection<CartItemDTO> Items { get; set; }
    }
}
