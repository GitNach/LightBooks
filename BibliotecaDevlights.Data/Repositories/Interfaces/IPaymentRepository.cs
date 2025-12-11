using BibliotecaDevlights.Data.Entities;

namespace BibliotecaDevlights.Data.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment?> GetByTransactionIdAsync(string transactionId);

    }
}
