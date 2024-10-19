using AutoMapper;
using Domain.Infrastructure.Interfaces;
using Domain.Models;
using Domain.Transfer;
using MediatR;

namespace Business.Commands
{
    public record AddItemToCartCommand(Guid CartId, CartItemDTO ItemDto) : IRequest<CartItemDTO?>;

    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, CartItemDTO?>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public AddItemToCartCommandHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartItemDTO?> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartAsync(request.CartId);
            if (cart == null)
                return null;

            var item = _mapper.Map<CartItem>(request.ItemDto);
            cart.AddItem(item);
            await _cartRepository.UpdateCartAsync(cart);
            return _mapper.Map<CartItemDTO>(item);
        }
    }

}
