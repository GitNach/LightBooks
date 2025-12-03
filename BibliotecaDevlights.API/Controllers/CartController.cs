using BibliotecaDevlights.Business.DTOs.Cart;
using BibliotecaDevlights.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDevlights.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDto>> GetCart(int userId)
        {
            var cart = await _cartService.GetOrCreateCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("{userId}/items")]
        public async Task<ActionResult<CartDto>> AddItemToCart(int userId, [FromBody] AddToCartDto addToCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cart = await _cartService.AddItemToCartAsync(userId, addToCart);
            return Ok(cart);
        }

        [HttpPut("{userId}/items/{itemId}")]
        public async Task<ActionResult<CartDto>> UpdateCartItem(int userId, int itemId, [FromBody] int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("La cantidad debe ser mayor que cero.");
            }
            var cart = await _cartService.UpdateCartItemQuantityAsync(userId, itemId, quantity);
            return Ok(cart);
        }

        [HttpDelete("{userId}/items/{itemId}")]
        public async Task<ActionResult<CartDto>> RemoveItemFromCart(int userId, int itemId)
        {
            var cart = await _cartService.RemoveItemFromCartAsync(userId, itemId);
            if (cart == null)
            {
                return NotFound("Item no encontrado en el carrito.");
            }
            return Ok(cart);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpPost("{userId}/validate-stock")]
        public async Task<IActionResult> ValidateCartStock(int userId)
        {
            await _cartService.ValidateCartStockAsync(userId);
            return Ok(new { message = "Stock validado correctamente." });
        }

        [HttpGet("{userId}/item-count")]
        public async Task<ActionResult<int>> GetCartItemCount(int userId)
        {
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Ok(count);
        }

        [HttpGet("{userId}/total")]
        public async Task<ActionResult<decimal>> GetCartTotal(int userId)
        {
            var cart = await _cartService.GetOrCreateCartAsync(userId);
            var total = await _cartService.GetCartTotalAsync(cart.Id);
            return Ok(total);
        }
    }
}
