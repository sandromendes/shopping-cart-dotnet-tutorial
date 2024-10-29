namespace Business.Pagination
{
    public class Filter
    {
        public decimal? MinPrice { get; set; } = 0;
        public decimal? MaxPrice { get; set; } = decimal.MaxValue;
    }
}
