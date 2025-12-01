using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDevlights.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Cart Cart { get; set; } = new Cart();
        public Book? Book
        {
            get; set;
        }
    }
}
