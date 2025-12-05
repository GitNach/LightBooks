using AutoMapper;
using BibliotecaDevlights.Business.DTOs.Order;
using BibliotecaDevlights.Data.Entities;

namespace BibliotecaDevlights.Business.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Cart -> Order (para CreateOrderFromCartAsync)
            CreateMap<Cart, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => 
                    src.CartItems!.Sum(ci => ci.Price * ci.Quantity)))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.CartItems));

            // CartItem -> OrderItem
            CreateMap<CartItem, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            // Order -> OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

            // OrderItem -> OrderItemDto
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => 
                    src.Book != null ? src.Book.Title : string.Empty));

            // Inversos (por si los necesitás)
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
        }
    }
}
