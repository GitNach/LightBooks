using BibliotecaDevlights.Business.DTOs.Payment;
using BibliotecaDevlights.Business.Services.Interfaces;
using BibliotecaDevlights.Data.Enums;
using BibliotecaDevlights.Data.Repositories.Interfaces;

namespace BibliotecaDevlights.Business.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IPaymentRepository paymentRespository, IOrderService orderService, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRespository;
            _orderService = orderService;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto paymentRequest)
        {
            var orderExists = await _orderRepository.ExistsAsync(paymentRequest.OrderId);
            if (!orderExists)
            {
                throw new KeyNotFoundException($"No se encontró la orden con ID {paymentRequest.OrderId}");
            }

            var random = new Random();
            var isSucces = random.Next(1, 11) <= 9;

            if (isSucces)
            {
                await _paymentRepository.AddAsync(new Data.Entities.Payment
                {
                    OrderId = paymentRequest.OrderId,
                    Amount = await _orderService.GetOrderTotalAmountAsync(paymentRequest.OrderId),
                    PaymentMethod = paymentRequest.PaymentMethod,
                    TransactionId = Guid.NewGuid().ToString(),
                    ProcessedAt = DateTime.UtcNow,
                    Status = OrderStatus.Completed.ToString()
                });
                return await SimulateSuccessfulPaymentAsync(paymentRequest.OrderId);
            }
            else
            {
                return await SimulateFailedPaymentAsync(paymentRequest.OrderId);
            }
        }

        public async Task<PaymentResultDto> SimulateSuccessfulPaymentAsync(int orderId)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Paid);
            return new PaymentResultDto
            {
                IsSuccess = true,
                TransactionId = Guid.NewGuid().ToString(),
                Message = "Pago procesado exitosamente",
                ProcessedAt = DateTime.UtcNow

            };
        }

        public async Task<PaymentResultDto> SimulateFailedPaymentAsync(int orderId)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Rejected);
            return new PaymentResultDto
            {
                IsSuccess = false,
                TransactionId = null,
                Message = "El pago fue rechazado, prueba nuevamente",
                ProcessedAt = DateTime.UtcNow

            };
        }

        public async Task<PaymentDto> GetPaymentByTransactionIdAsync(string transactionId)
        {
            var payment = await _paymentRepository.GetByTransactionIdAsync(transactionId);

            if (payment == null)
            {
                throw new KeyNotFoundException($"No se encontró el pago con TransactionId {transactionId}");
            }

            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                TransactionId = payment.TransactionId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt,
                ProcessedAt = payment.ProcessedAt,
                Message = payment.Message,
                ErrorCode = payment.ErrorCode
            };
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);

            if (payment == null)
            {
                throw new KeyNotFoundException($"No se encontró el pago con ID {paymentId}");
            }

            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                TransactionId = payment.TransactionId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt,
                ProcessedAt = payment.ProcessedAt,
                Message = payment.Message,
                ErrorCode = payment.ErrorCode
            };
        }
    }
}
