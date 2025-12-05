namespace BibliotecaDevlights.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; } = "Purchase";
        public Cart Cart { get; set; } = null!;
        public Book? Book
        {
            get; set;
        }
    }
}
