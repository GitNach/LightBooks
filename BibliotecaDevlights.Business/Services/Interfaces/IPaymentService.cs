using BibliotecaDevlights.Business.DTOs.Payment;

namespace BibliotecaDevlights.Business.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto paymentRequest);
        Task<PaymentDto> GetPaymentByTransactionIdAsync(string transactionId);
        Task<PaymentDto> GetPaymentByIdAsync(int paymentId);
        Task<PaymentResultDto> SimulateSuccessfulPaymentAsync(int orderId);
        Task<PaymentResultDto> SimulateFailedPaymentAsync(int orderId);
    }
}
