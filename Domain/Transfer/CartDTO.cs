namespace Domain.Mapping
{
    public class CartDTO
    {
        public int Id { get; set; }

        public ICollection<CartItemDTO> Items { get; set; }
    }
}
