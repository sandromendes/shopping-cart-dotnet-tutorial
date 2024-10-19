using Domain.Infrastructure.Interfaces;
using MediatR;

namespace Business.Commands
{
    public record DeleteCartCommand(Guid CartId) : IRequest<bool>;

    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public DeleteCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartAsync(request.CartId);
            if (cart == null)
                return false;

            await _cartRepository.DeleteCartAsync(request.CartId);
            return true;
        }
    }
}
