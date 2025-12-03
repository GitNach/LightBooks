using BibliotecaDevlights.Business.DTOs.Cart;
using BibliotecaDevlights.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaDevlights.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            return int.Parse(userIdClaim.Value);
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            var userId = GetCurrentUserId();
            var cart = await _cartService.GetOrCreateCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartDto>> AddItemToCart([FromBody] AddToCartDto addToCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = GetCurrentUserId();
            var cart = await _cartService.AddItemToCartAsync(userId, addToCart);
            return Ok(cart);
        }

        [HttpPut("items/{itemId}")]
        public async Task<ActionResult<CartDto>> UpdateCartItem(int itemId, [FromBody] int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("La cantidad debe ser mayor que cero.");
            }
            var userId = GetCurrentUserId();
            var cart = await _cartService.UpdateCartItemQuantityAsync(userId, itemId, quantity);
            return Ok(cart);
        }

        [HttpDelete("items/{itemId}")]
        public async Task<ActionResult<CartDto>> RemoveItemFromCart(int itemId)
        {
            var userId = GetCurrentUserId();
            var cart = await _cartService.RemoveItemFromCartAsync(userId, itemId);
            if (cart == null)
            {
                return NotFound("Item no encontrado en el carrito.");
            }
            return Ok(cart);
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpPost("validate-stock")]
        public async Task<IActionResult> ValidateCartStock()
        {
            var userId = GetCurrentUserId();
            await _cartService.ValidateCartStockAsync(userId);
            return Ok(new { message = "Stock validado correctamente." });
        }

        [HttpGet("item-count")]
        public async Task<ActionResult<int>> GetCartItemCount()
        {
            var userId = GetCurrentUserId();
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Ok(count);
        }

        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetCartTotal()
        {
            var userId = GetCurrentUserId();
            var cart = await _cartService.GetOrCreateCartAsync(userId);
            var total = await _cartService.GetCartTotalAsync(cart.Id);
            return Ok(total);
        }
    }
}
