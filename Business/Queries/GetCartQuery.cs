using AutoMapper;
using Domain.Infrastructure.Interfaces;
using Domain.Transfer;
using MediatR;

namespace Business.Queries
{
    public record GetCartQuery(Guid CartId) : IRequest<CartDTO?>;

    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartDTO?>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDTO?> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartAsync(request.CartId);
            return _mapper.Map<CartDTO?>(cart);
        }
    }
}
