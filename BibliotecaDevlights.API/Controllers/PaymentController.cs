using BibliotecaDevlights.Business.DTOs.Payment;
using BibliotecaDevlights.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDevlights.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<ActionResult<PaymentResultDto>> ProcessPayment([FromBody] PaymentRequestDto paymentRequest)
        {
            try
            {
                var result = await _paymentService.ProcessPaymentAsync(paymentRequest);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al procesar el pago", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPaymentById(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                return Ok(payment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el pago", error = ex.Message });
            }
        }

        [HttpGet("transaction/{transactionId}")]
        public async Task<ActionResult<PaymentDto>> GetPaymentByTransactionId(string transactionId)
        {
            try
            {
                var result = await _paymentService.GetPaymentByTransactionIdAsync(transactionId);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el pago por transacción", error = ex.Message });
            }
        }

        [HttpPost("simulate/success/{orderId}")]
        public async Task<ActionResult<PaymentResultDto>> SimulateSuccessfulPayment(int orderId)
        {
            try
            {
                var result = await _paymentService.SimulateSuccessfulPaymentAsync(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al simular pago exitoso", error = ex.Message });
            }
        }

        [HttpPost("simulate/failed/{orderId}")]
        public async Task<ActionResult<PaymentResultDto>> SimulateFailedPayment(int orderId)
        {
            try
            {
                var result = await _paymentService.SimulateFailedPaymentAsync(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al simular pago fallido", error = ex.Message });
            }
        }
    }
}
