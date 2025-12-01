using BibliotecaDevlights.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDevlights.Data.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart> CreateCartAsync(int userId);
        Task ClearCartAsync(int userId);

        // ===== CART ITEMS =====
        Task<CartItem?> GetCartItemAsync(int cartId, int bookId);
        Task AddItemToCartAsync(CartItem cartItem);
        Task UpdateCartItemQuantityAsync(int cartItemId, int quantity);
        Task RemoveItemFromCartAsync(int cartItemId);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);

        // ===== UTILIDADES =====
        Task<bool> CartExistsForUserAsync(int userId);
        Task<int> GetCartItemCountAsync(int cartId);
        Task<decimal> GetCartTotalAsync(int cartId);

    }
}
