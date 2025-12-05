using BibliotecaDevlights.Business.DTOs.Order;
using BibliotecaDevlights.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDevlights.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        [HttpGet("{orderId}/user/{userId}")]
        public async Task<ActionResult<OrderDto?>> GetOrderById(int orderId, int userId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId, userId);
            return Ok(order);
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }
        [HttpGet("details")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrdersWithDetails()
        {
            var orders = await _orderService.GetAllOrdersWithDetailsAsync();
            return Ok(orders);
        }
        [HttpPost("user/{userId}")]
        public async Task<ActionResult<OrderDto>> CreateOrder(int userId)
        {
            var order = await _orderService.CreateOrderFromCartAsync(userId);
            return Ok(order);
        }
    }
}
