using Domain.Transfer;

namespace Business.Pagination
{
    public class Ordering
    {
        public string SortField { get; set; } = nameof(CartItemDTO.Price);
        public bool IsAscending { get; set; } = true;
    }
}
