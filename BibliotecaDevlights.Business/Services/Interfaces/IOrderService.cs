using BibliotecaDevlights.Business.DTOs.Order;
using BibliotecaDevlights.Data.Enums;

namespace BibliotecaDevlights.Business.Services.Interfaces
{
    public interface IOrderService
    {
        // Crear orden desde carrito (flujo principal)
        Task<OrderDto> CreateOrderFromCartAsync(int userId);

        // Consultas
        Task<OrderDto?> GetOrderByIdAsync(int orderId, int userId);
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync(); // Para admin
        Task<IEnumerable<OrderDto>> GetAllOrdersWithDetailsAsync();
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdWithDetailsAsync(int userId);

        // Gestión de estados
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> CancelOrderAsync(int orderId, int userId);

        // Validaciones
        Task<bool> UserOwnsOrderAsync(int orderId, int userId);
    }
}
