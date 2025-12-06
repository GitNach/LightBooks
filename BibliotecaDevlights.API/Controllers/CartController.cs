using BibliotecaDevlights.Business.DTOs.Cart;
using BibliotecaDevlights.Business.Services.Interfaces;
using BibliotecaDevlights.Business.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDevlights.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserContextService _userContextService;

        public CartController(ICartService cartService, IUserContextService userContextService)
        {
            _cartService = cartService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            var userId = _userContextService.GetUserId();
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
            var userId = _userContextService.GetUserId();
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
            var userId = _userContextService.GetUserId();
            var cart = await _cartService.UpdateCartItemQuantityAsync(userId, itemId, quantity);
            return Ok(cart);
        }

        [HttpDelete("items/{itemId}")]
        public async Task<ActionResult<CartDto>> RemoveItemFromCart(int itemId)
        {
            var userId = _userContextService.GetUserId();
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
            var userId = _userContextService.GetUserId();
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpPost("validate-stock")]
        public async Task<IActionResult> ValidateCartStock()
        {
            var userId = _userContextService.GetUserId();
            await _cartService.ValidateCartStockAsync(userId);
            return Ok(new { message = "Stock validado correctamente." });
        }

        [HttpGet("item-count")]
        public async Task<ActionResult<int>> GetCartItemCount()
        {
            var userId = _userContextService.GetUserId();
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Ok(count);
        }

        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetCartTotal()
        {
            var userId = _userContextService.GetUserId();
            var total = await _cartService.GetCartTotalAsync(userId);
            return Ok(total);
        }
    }
}
