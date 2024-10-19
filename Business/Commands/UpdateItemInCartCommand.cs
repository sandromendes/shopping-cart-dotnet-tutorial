using AutoMapper;
using Domain.Infrastructure.Interfaces;
using Domain.Transfer;
using MediatR;

namespace Business.Commands
{
    public record UpdateItemInCartCommand(Guid CartId, Guid ItemId, CartItemDTO ItemDto) : IRequest<CartItemDTO?>;

    public class UpdateItemInCartCommandHandler : IRequestHandler<UpdateItemInCartCommand, CartItemDTO?>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public UpdateItemInCartCommandHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartItemDTO?> Handle(UpdateItemInCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartAsync(request.CartId);
            if (cart == null)
                return null;

            var item = cart.Items.FirstOrDefault(i => i.Id == request.ItemId);
            if (item == null)
                return null;

            item.UpdateQuantity(request.ItemDto.Quantity);
            await _cartRepository.UpdateCartAsync(cart);

            return _mapper.Map<CartItemDTO>(item);
        }
    }
}
