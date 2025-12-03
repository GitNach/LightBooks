using AutoMapper;
using BibliotecaDevlights.Business.DTOs.Cart;
using BibliotecaDevlights.Business.Services.Interfaces;
using BibliotecaDevlights.Data.Entities;
using BibliotecaDevlights.Data.Repositories.Interfaces;

namespace BibliotecaDevlights.Business.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<CartDto?> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return null;
            
            var cartDto = _mapper.Map<CartDto>(cart);
            cartDto.TotalPrice = await _cartRepository.GetCartTotalAsync(cart.Id);
            cartDto.ItemCount = await _cartRepository.GetCartItemCountAsync(cart.Id);
            
            return cartDto;
        }

        public async Task<CartDto> GetOrCreateCartAsync(int userId)
        {
            if (!await _cartRepository.CartExistsForUserAsync(userId))
            {
                var newCart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    TotalPrice = 0
                };
                var cart = await _cartRepository.CreateCartAsync(newCart);
                return _mapper.Map<CartDto>(cart);
            }
            
            var existingCart = await _cartRepository.GetCartByUserIdAsync(userId);
            return _mapper.Map<CartDto>(existingCart);
        }

        public async Task<CartDto> AddItemToCartAsync(int userId, AddToCartDto addToCartDto)
        {
            var canAdd = await CanAddItemToCartAsync(userId, addToCartDto.BookId, addToCartDto.Quantity, addToCartDto.Type);
            if (!canAdd)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible");
            }

            var cart = await GetOrCreateCartAsync(userId);

            var book = await _bookRepository.GetByIdAsync(addToCartDto.BookId);
            if (book == null)
            {
                throw new InvalidOperationException("Libro no encontrado");
            }

            var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, addToCartDto.BookId);
            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + addToCartDto.Quantity;
                await _cartRepository.UpdateCartItemQuantityAsync(existingItem.Id, newQuantity);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    BookId = addToCartDto.BookId,
                    Quantity = addToCartDto.Quantity,
                    Price = addToCartDto.Type == "Purchase" ? book.PurchasePrice : book.RentalPricePerDay
                };
                await _cartRepository.AddItemToCartAsync(cartItem);
            }

            return await GetCartByUserIdAsync(userId) ?? throw new InvalidOperationException("Error al obtener el carrito actualizado");
        }

        public async Task<CartDto> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a 0");
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new InvalidOperationException("Carrito no encontrado");
            }

            await _cartRepository.UpdateCartItemQuantityAsync(cartItemId, quantity);

            return await GetCartByUserIdAsync(userId) ?? throw new InvalidOperationException("Error al obtener el carrito actualizado");
        }

        public async Task<CartDto> RemoveItemFromCartAsync(int userId, int cartItemId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new InvalidOperationException("Carrito no encontrado");
            }

            await _cartRepository.RemoveItemFromCartAsync(cartItemId);

            return await GetCartByUserIdAsync(userId) ?? throw new InvalidOperationException("Error al obtener el carrito actualizado");
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new InvalidOperationException("Carrito no encontrado");
            }

            await _cartRepository.ClearCartAsync(userId);
        }

        public async Task ValidateCartStockAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new InvalidOperationException("Carrito no encontrado");
            }

            var cartItems = await _cartRepository.GetCartItemsAsync(cart.Id);

            foreach (var item in cartItems)
            {
                var book = await _bookRepository.GetByIdAsync(item.BookId);
                if (book == null)
                {
                    throw new InvalidOperationException($"Libro con ID {item.BookId} no encontrado");
                }

                var isPurchase = item.Price == book.PurchasePrice;
                var availableStock = isPurchase ? book.StockPurchase : book.StockRental;

                if (item.Quantity > availableStock)
                {
                    throw new InvalidOperationException(
                        $"Stock insuficiente para '{book.Title}'. Disponible: {availableStock}, Solicitado: {item.Quantity}");
                }
            }
        }

        public async Task<bool> CanAddItemToCartAsync(int userId, int bookId, int quantity, string type)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                return false;
            }

            var availableStock = type == "Purchase" ? book.StockPurchase : book.StockRental;

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, bookId);
                if (existingItem != null)
                {
                    var totalQuantity = existingItem.Quantity + quantity;
                    return totalQuantity <= availableStock;
                }
            }

            return quantity <= availableStock;
        }

        public async Task<int> GetCartItemCountAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return 0;
            }
            return await _cartRepository.GetCartItemCountAsync(cart.Id);
        }
        public async Task<decimal> GetCartTotalAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(cartId);
            if (cart == null)
            {
                throw new InvalidOperationException("Carrito no encontrado");
            }
            return await _cartRepository.GetCartTotalAsync(cartId);
        }
    }
}
