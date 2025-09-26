using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Stripe.Data;

namespace YourApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly AppDbContext _context;
        private const string endpointSecret = "whsec_3f91588ffbc1758db0aaae95f36c399096a43ca8349e306258c2c1985b03f652"; // ضع سيكرتك هنا

        public StripeWebhookController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            string json;
            try
            {
                Console.WriteLine("Received webhook");
                json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    endpointSecret
                );

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    if (session != null && session.Metadata.ContainsKey("cartId"))
                    {
                        int cartId = int.Parse(session.Metadata["cartId"]);
                        var cart = await _context.Carts.FindAsync(cartId);
                        if (cart != null)
                        {
                            cart.Status = "Paid";
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
