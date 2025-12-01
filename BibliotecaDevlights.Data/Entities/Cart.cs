namespace BibliotecaDevlights.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public decimal TotalPrice { get; set; }
        // Navigation property
        public User? User { get; set; }
        public ICollection<CartItem>? CartItems
        {
            get; set;
        }
    }
}
