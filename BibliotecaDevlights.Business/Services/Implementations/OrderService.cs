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

        public async Task<IEnumerable<OrderSimpleDto>> GetAllOrdersAsync()
        {
            var order = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderSimpleDto>>(order);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }
            if (!await UserOwnsOrderAsync(orderId, userId))
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

            var order = new Order
            {
                UserId = cart.UserId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            try
            {
                foreach (var ci in cart.CartItems)
                {
                    var book = await _bookRepository.GetByIdAsync(ci.BookId);
                    if (book == null)
                    {
                        throw new InvalidOperationException($"Libro con ID {ci.BookId} no encontrado");
                    }

                    var orderItem = new OrderItem
                    {
                        BookId = ci.BookId,
                        Book = book,
                        Quantity = ci.Quantity,
                        Price = ci.Price,
                        Type = ci.Type,
                        RentalStartDate = ci.RentalStartDate,
                        RentalEndDate = ci.RentalEndDate
                    };

                    order.OrderItems!.Add(orderItem);

                    if (ci.Type == TransactionType.Rental && ci.RentalStartDate.HasValue && ci.RentalEndDate.HasValue)
                    {
                        int rentalDays = (ci.RentalEndDate.Value.Date - ci.RentalStartDate.Value.Date).Days;
                        rentalDays = Math.Max(rentalDays, 1);
                        totalAmount += ci.Price * ci.Quantity * rentalDays;
                    }
                    else
                    {
                        totalAmount += ci.Price * ci.Quantity;
                    }

                    var isPurchase = ci.Type == TransactionType.Purchase;
                    if (isPurchase)
                        book.StockPurchase -= ci.Quantity;
                    else
                        book.StockRental -= ci.Quantity;

                    await _bookRepository.UpdateAsync(book);
                }

                order.TotalAmount = totalAmount;
                await _orderRepository.AddAsync(order);
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
            if (!await UserOwnsOrderAsync(orderId, userId))
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
                        var isPurchase = orderItem.Type == TransactionType.Purchase;
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


        public async Task MarkAsReturnedAsync(int orderId, int userId)
        {
            if (!await _orderRepository.ExistsAsync(orderId))
            {
                throw new KeyNotFoundException("Order not found");
            }

            if (!await UserOwnsOrderAsync(orderId, userId))
            {
                throw new UnauthorizedAccessException("User does not own the order");
            }

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order?.OrderItems == null || !order.OrderItems.Any())
            {
                throw new InvalidOperationException("Order has no items");
            }

            var rentalItems = order.OrderItems
                .Where(oi => oi.Type == TransactionType.Rental && !oi.IsReturned)
                .ToList();

            if (!rentalItems.Any())
            {
                throw new InvalidOperationException("No rental items found to return");
            }

            foreach (var rentalItem in rentalItems)
            {
                rentalItem.IsReturned = true;
                rentalItem.RentalReturnedDate = DateTime.UtcNow;

                var book = await _bookRepository.GetByIdAsync(rentalItem.BookId);
                if (book != null)
                {
                    book.StockRental += rentalItem.Quantity;
                    await _bookRepository.UpdateAsync(book);
                }
            }
            order.Status= OrderStatus.Completed;
            await _orderRepository.UpdateAsync(order);
        }
        
        public async Task<IEnumerable<OrderDto>> GetActiveRentalsAsync(int userId)
        {
            var order = await _orderRepository.GetActiveRentalsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOverdueRentalsAsync(int userId)
        {
            var order = await _orderRepository.GetOverdueRentalsAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(order);
        }


        public async Task<bool> UserOwnsOrderAsync(int orderId, int userId)
        {
            if (await _orderRepository.ExistsAsync(orderId))
            {
                return await _orderRepository.UserOwnsOrderAsync(orderId, userId);
            }
            return false;
        }

        public async Task<decimal> GetOrderTotalAmountAsync(int orderId)
        {
            var order =  await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }
            return order.TotalAmount;
        }

    }
}
