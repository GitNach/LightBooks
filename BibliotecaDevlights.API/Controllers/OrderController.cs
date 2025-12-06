using BibliotecaDevlights.Business.DTOs.Order;
using BibliotecaDevlights.Business.Services.Interfaces;
using BibliotecaDevlights.Business.Utilities;
using BibliotecaDevlights.Data.Enums;
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
        private readonly IUserContextService _userContextService;

        public OrderController(IOrderService orderService, IUserContextService userContextService)
        {
            _orderService = orderService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDto?>> GetOrderById(int orderId)
        {
            var userId = _userContextService.GetUserId();
            var order = await _orderService.GetOrderByIdAsync(orderId, userId);
            return Ok(order);
        }

        [HttpGet("user/my-orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders()
        {
            var userId = _userContextService.GetUserId();
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet("details")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrdersWithDetails()
        {
            var orders = await _orderService.GetAllOrdersWithDetailsAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder()
        {
            var userId = _userContextService.GetUserId();
            var order = await _orderService.CreateOrderFromCartAsync(userId);
            return Ok(order);
        }

        [HttpPut("{orderId}/status/{status}")]
        public async Task<ActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, status);
            return NoContent();
        }

        [HttpDelete("{orderId}")]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var userId = _userContextService.GetUserId();
            await _orderService.CancelOrderAsync(orderId, userId);
            return NoContent();
        }
    }
}
