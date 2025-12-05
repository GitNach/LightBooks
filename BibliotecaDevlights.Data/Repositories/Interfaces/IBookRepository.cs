using BibliotecaDevlights.Data.Entities;

namespace BibliotecaDevlights.Data.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task AddAsync(Book newBook);
        Task<Book?> UpdateAsync(int id, Book updatedBook);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<Book>> GetByAuthorAsync(int authorId);
        Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);

        Task UpdateAsync(Book book);

    }
}
