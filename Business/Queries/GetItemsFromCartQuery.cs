using AutoMapper;
using Domain.Infrastructure.Interfaces;
using Domain.Transfer;
using MediatR;

namespace Business.Queries
{
    public record GetItemsFromCartQuery(Guid CartId) : IRequest<IEnumerable<CartItemDTO>?>;

    public class GetItemsFromCartQueryHandler : IRequestHandler<GetItemsFromCartQuery, IEnumerable<CartItemDTO>?>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetItemsFromCartQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDTO>?> Handle(GetItemsFromCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartAsync(request.CartId);
            if (cart == null)
                return null;

            return _mapper.Map<IEnumerable<CartItemDTO>>(cart.Items);
        }
    }
}
