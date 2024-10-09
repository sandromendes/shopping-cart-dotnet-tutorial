using AutoMapper;
using Domain.Models;
using Domain.Transfer;

namespace Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CartItem, CartItemDTO>();
                        
            CreateMap<CartDTO, Cart>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
                        
            CreateMap<CartItemDTO, CartItem>()
                .ConstructUsing(src => new CartItem(src.ProductId, src.ProductName, src.Quantity, src.Price));
        }
    }
}
