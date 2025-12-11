using BibliotecaDevlights.Business.DTOs.Order;
using BibliotecaDevlights.Data.Enums;

namespace BibliotecaDevlights.Business.Services.Interfaces
{
    public interface IOrderService
    {

        Task<OrderDto> CreateOrderFromCartAsync(int userId);


        Task<OrderDto?> GetOrderByIdAsync(int orderId, int userId);
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderSimpleDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderDto>> GetAllOrdersWithDetailsAsync();
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdWithDetailsAsync(int userId);


        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> CancelOrderAsync(int orderId, int userId);

        //Rental
        Task MarkAsReturnedAsync(int orderId, int userId);
        Task<IEnumerable<OrderDto>> GetActiveRentalsAsync(int userId);
        Task<IEnumerable<OrderDto>> GetOverdueRentalsAsync(int userId);


        Task<bool> UserOwnsOrderAsync(int orderId, int userId);

        Task<decimal> GetOrderTotalAmountAsync(int orderId);
    }
}
