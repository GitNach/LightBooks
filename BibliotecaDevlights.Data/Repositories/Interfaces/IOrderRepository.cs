using BibliotecaDevlights.Data.Entities;
using BibliotecaDevlights.Data.Enums;

namespace BibliotecaDevlights.Data.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByUserIdWithDetailsAsync(int userId);
        Task<Order?> GetByIdAsync(int orderId);
        Task AddAsync(Order order);
        Task<bool> DeleteAsync(int orderId);

        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> ExistsAsync(int id);
        Task<bool> UserOwnsOrderAsync(int orderId, int userId);



    }
}
