namespace Domain.Transfer
{
    public class CartDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public ICollection<CartItemDTO> Items { get; set; }
    }
}
