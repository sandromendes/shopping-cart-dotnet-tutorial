using AutoMapper;
using Domain.Business.Interfaces;
using Domain.Infrastructure.Interfaces;
using Domain.Mapping;
using Domain.Models;

namespace Business.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDTO> GetCartAsync(int id)
        {
            var cart = await _cartRepository.GetCartAsync(id);
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO> AddCartAsync(CartDTO cartDto)
        {
            if (cartDto == null) throw new ArgumentNullException(nameof(cartDto));

            var cart = _mapper.Map<Cart>(cartDto);
            await _cartRepository.AddCartAsync(cart);
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO> UpdateCartAsync(CartDTO cartDto)
        {
            if (cartDto == null) throw new ArgumentNullException(nameof(cartDto));

            var cart = await _cartRepository.GetCartAsync(cartDto.Id);
            if (cart == null)
                return null;

            _mapper.Map(cartDto, cart);
            await _cartRepository.UpdateCartAsync(cart);
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<bool> DeleteCartAsync(int id)
        {
            var cart = await _cartRepository.GetCartAsync(id);
            if (cart == null)
                return false;

            await _cartRepository.DeleteCartAsync(id);
            return true;
        }

        public async Task<IEnumerable<CartItemDTO>> GetItemsFromCartAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart == null)
                return null;

            return _mapper.Map<IEnumerable<CartItemDTO>>(cart.Items);
        }

        public async Task<CartItemDTO> AddItemToCartAsync(int cartId, CartItemDTO itemDto)
        {
            if (itemDto == null) throw new ArgumentNullException(nameof(itemDto));

            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart == null)
                return null;

            var item = _mapper.Map<CartItem>(itemDto);
            cart.AddItem(item);
            await _cartRepository.UpdateCartAsync(cart);
            return _mapper.Map<CartItemDTO>(item);
        }

        public async Task<bool> RemoveItemFromCartAsync(int cartId, int itemId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart == null)
                return false;

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                return false;

            cart.RemoveItem(item.ProductId);
            await _cartRepository.UpdateCartAsync(cart);
            return true;
        }

        public async Task<CartItemDTO> UpdateItemInCartAsync(int cartId, int itemId, CartItemDTO itemDto)
        {
            if (itemDto == null) throw new ArgumentNullException(nameof(itemDto));

            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart == null)
                return null;

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                return null;

            _mapper.Map(itemDto, item);
            await _cartRepository.UpdateCartAsync(cart);
            return _mapper.Map<CartItemDTO>(item);
        }
    }
}
