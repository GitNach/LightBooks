using AutoMapper;
using BibliotecaDevlights.Business.DTOs.Order;
using BibliotecaDevlights.Business.Services.Interfaces;
using BibliotecaDevlights.Data.Entities;
using BibliotecaDevlights.Data.Enums;
using BibliotecaDevlights.Data.Repositories.Interfaces;

namespace BibliotecaDevlights.Business.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IBookRepository bookRepository,
            IMapper mapper,
            ICartService cartService)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _cartService = cartService;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var order = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(order);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }
            if (!await _orderRepository.UserOwnsOrderAsync(orderId, userId))
            {
                throw new UnauthorizedAccessException("User does not own the order");
            }
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var order = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersWithDetailsAsync()
        {
            var orders = await _orderRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdWithDetailsAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdWithDetailsAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateOrderFromCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("El carrito está vacío");
            }

            await _cartService.ValidateCartStockAsync(userId);

            var order = _mapper.Map<Order>(cart);
            order.UserId = cart.UserId;
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;

            try
            {
                await _orderRepository.AddAsync(order);

                foreach (var item in cart.CartItems)
                {
                    var book = await _bookRepository.GetByIdAsync(item.BookId);
                    if (book != null)
                    {
                        var isPurchase = item.Type == "Purchase";
                        if (isPurchase)
                            book.StockPurchase -= item.Quantity;
                        else
                            book.StockRental -= item.Quantity;

                        await _bookRepository.UpdateAsync(book);
                    }
                }
                await _cartRepository.ClearCartAsync(userId);

                return _mapper.Map<OrderDto>(order);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al crear la orden. No se realizaron cambios.", ex);
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            if (!await _orderRepository.ExistsAsync(orderId))
            {
                throw new KeyNotFoundException("Order not found");
            }
            return await _orderRepository.UpdateOrderStatusAsync(orderId, status);
        }

        public async Task<bool> CancelOrderAsync(int orderId, int userId)
        {
            if (!await _orderRepository.ExistsAsync(orderId))
            {
                return false;
            }
            if (!await _orderRepository.UserOwnsOrderAsync(orderId, userId))
            {
                throw new UnauthorizedAccessException("User does not own the order");
            }
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order?.OrderItems != null)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    var book = await _bookRepository.GetByIdAsync(orderItem.BookId);
                    if (book != null)
                    {
                        var isPurchase = orderItem.Price == book.PurchasePrice;
                        if (isPurchase)
                            book.StockPurchase += orderItem.Quantity;
                        else
                            book.StockRental += orderItem.Quantity;

                        await _bookRepository.UpdateAsync(book);
                    }
                }
            }

            return await _orderRepository.DeleteAsync(orderId);
        }

        public async Task<bool> UserOwnsOrderAsync(int orderId, int userId)
        {
            if (await _orderRepository.ExistsAsync(orderId))
            {
                return await _orderRepository.UserOwnsOrderAsync(orderId, userId);
            }
            return false;
        }
    }
}
