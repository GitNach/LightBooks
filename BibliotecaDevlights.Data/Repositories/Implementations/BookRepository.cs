using BibliotecaDevlights.Data.Data;
using BibliotecaDevlights.Data.Entities;
using BibliotecaDevlights.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDevlights.Data.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.AsNoTracking().ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.Include(c => c.Category).Include(a => a.Author).FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task AddAsync(Book newBook)
        {
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
        }
        public async Task<Book?> UpdateAsync(int id, Book updatedBook)
        {
            if (updatedBook == null)
                return null;

            var existingBook = await GetByIdAsync(id);
            if (existingBook == null)
                return null;

            existingBook.Title = updatedBook.Title;
            existingBook.ISBN = updatedBook.ISBN;
            existingBook.Description = updatedBook.Description;
            existingBook.PurchasePrice = updatedBook.PurchasePrice;
            existingBook.RentalPricePerDay = updatedBook.RentalPricePerDay;
            existingBook.StockPurchase = updatedBook.StockPurchase;
            existingBook.StockRental = updatedBook.StockRental;
            existingBook.ImageUrl = updatedBook.ImageUrl;
            existingBook.PublishedDate = updatedBook.PublishedDate;
            existingBook.CategoryId = updatedBook.CategoryId;
            existingBook.AuthorId = updatedBook.AuthorId;

            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);
            if (book == null)
            {
                return false;
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Book>> GetByAuthorAsync(int authorId)
        {
            return await _context.Books.AsNoTracking()
                .Where(b => b.AuthorId == authorId).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Books.AsNoTracking()
                .Where(b => b.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var lowerSearchTerm = searchTerm.ToLower();
            var books = await _context.Books.AsNoTracking()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .ToListAsync();

            return books
                .Where(b => b.Title.ToLower().Contains(lowerSearchTerm) ||
                            b.ISBN.ToLower().Contains(lowerSearchTerm) ||
                            (b.Description != null && b.Description.ToLower().Contains(lowerSearchTerm)) ||
                            (b.Author != null && (b.Author.FirstName.ToLower().Contains(lowerSearchTerm) ||
                                                  b.Author.LastName.ToLower().Contains(lowerSearchTerm))) ||
                            (b.Category != null && b.Category.Name.ToLower().Contains(lowerSearchTerm)) ||
                            (b.PublishedDate.HasValue && b.PublishedDate.Value.ToString("dd-MM-yyyy").Contains(lowerSearchTerm)) ||
                            (b.StockPurchase > 0).ToString().ToLower().Contains(lowerSearchTerm) ||
                            (b.StockRental > 0).ToString().ToLower().Contains(lowerSearchTerm))
                .ToList();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

    }
}
