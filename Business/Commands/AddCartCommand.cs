using AutoMapper;
using Domain.Infrastructure.Interfaces;
using Domain.Models;
using Domain.Transfer;
using MediatR;

namespace Business.Commands
{
    public record AddCartCommand(CartDTO CartDto) : IRequest<CartDTO>;

    public class AddCartCommandHandler : IRequestHandler<AddCartCommand, CartDTO>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public AddCartCommandHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDTO> Handle(AddCartCommand request, CancellationToken cancellationToken)
        {
            var cart = _mapper.Map<Cart>(request.CartDto);
            await _cartRepository.AddCartAsync(cart);
            return _mapper.Map<CartDTO>(cart);
        }
    }
}
