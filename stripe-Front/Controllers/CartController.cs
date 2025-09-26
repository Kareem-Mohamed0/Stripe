using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Interfaces;
using Stripe.Models;

namespace Stripe.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<AppUser> _userManager;

        public CartController(ICartRepository cartRepository, UserManager<AppUser> userManager)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();

            var cart = await _cartRepository.GetUserCartAsync(userId);
            return Ok(cart);
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            await _cartRepository.AddToCartAsync(userId, productId, quantity);
            return Ok("Item added to cart");
        }
    }

}
