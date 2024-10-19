using Domain.Infrastructure.Interfaces;
using MediatR;

namespace Business.Commands
{
    public record RemoveItemFromCartCommand(Guid CartId, Guid ItemId) : IRequest<bool>;

    public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public RemoveItemFromCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartAsync(request.CartId);
            if (cart == null)
                return false;

            var item = cart.Items.FirstOrDefault(i => i.Id == request.ItemId);
            if (item == null)
                return false;

            cart.RemoveItem(item.ProductId);
            await _cartRepository.UpdateCartAsync(cart);
            return true;
        }
    }
}
