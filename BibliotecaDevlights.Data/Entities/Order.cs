using BibliotecaDevlights.Data.Enums;

namespace BibliotecaDevlights.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public User? User { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; } 
    }
}
