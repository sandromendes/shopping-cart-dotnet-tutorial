using AutoMapper;
using Business.Pagination;
using Domain.Infrastructure.Interfaces;
using Domain.Models;
using Domain.Transfer;
using MediatR;

namespace Business.Queries
{
    public class GetItemsFromCartQuery : IRequest<PagedResult<CartItemDTO>?>
    {
        public Guid CartId { get; set; }
        public PageInfo PageInfo { get; set; }
        public Ordering Ordering { get; set; }
        public Filter Filter { get; set; }

        public GetItemsFromCartQuery(Guid cartId, PageInfo pageInfo, Ordering ordering, Filter filter)
        {
            CartId = cartId;
            PageInfo = pageInfo;
            Ordering = ordering;
            Filter = filter;
        }
    }

    public class GetItemsFromCartQueryHandler : IRequestHandler<GetItemsFromCartQuery, PagedResult<CartItemDTO>?>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetItemsFromCartQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CartItemDTO>?> Handle(GetItemsFromCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartAsync(request.CartId);

            if (cart == null)
                return null;

            var items = cart.Items.AsQueryable();

            var totalItems = items.Count();

            if (request.Filter.MinPrice > 0)
                items = items.Where(i => !request.Filter.MinPrice.HasValue || i.Price >= request.Filter.MinPrice);

            if(request.Filter.MaxPrice < decimal.MaxValue)
                items = items.Where(i => !request.Filter.MaxPrice.HasValue || i.Price <= request.Filter.MaxPrice);

            if (!string.IsNullOrEmpty(request.Ordering.SortField))
            {
                var propertyInfo = typeof(CartItem).GetProperty(request.Ordering.SortField);

                if (propertyInfo != null)
                {
                    items = request.Ordering.IsAscending
                        ? items.OrderBy(item => propertyInfo.GetValue(item, null))
                        : items.OrderByDescending(item => propertyInfo.GetValue(item, null));
                }
            }

            items = items
                .Skip((request.PageInfo.Page - 1) * request.PageInfo.PageSize)
                .Take(request.PageInfo.PageSize);

            var result = _mapper.Map<IEnumerable<CartItemDTO>>(items);

            var pagedResult = new PagedResult<CartItemDTO>(result, totalItems, request.PageInfo.Page, request.PageInfo.PageSize);

            return pagedResult;
        }
    }
}
