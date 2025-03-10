using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimoshStoreAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        
        [HttpGet("/api/order-checkout/{userId}")]
        public async Task<IActionResult> CheckOut(int userId)
        {
            var model = await _cartService.CheckOut(userId);

            return Ok(model);
        }
        [HttpDelete("/api/clear-cart/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var result = await _cartService.ClearCartAsync(userId);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);

        }
        [HttpPut("/api/remove-from-cart/{userId}")]
        public async Task<IActionResult> Delete([FromBody]int productId, int userId)
        {
            var result = await _cartService.DeleteCartItem(userId, productId);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
        [HttpPost("/api/add-to-cart/{userId}")]
        public async Task<IActionResult> AddCart(int userId, CartDTO dto)
        {
            var result = await _cartService.AddToCart(userId, dto);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
        [HttpGet("/api/{id}/cart-items")]
        public async Task<IActionResult> CartItems(int id)
        {
            var cartItems = await _cartService.GetCartItems(id);

            return Ok(cartItems);
        }
        
    }
}
