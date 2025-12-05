namespace BibliotecaDevlights.Business.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public ICollection<OrderItemDto> Items { get; set; } = [];
    }
}
