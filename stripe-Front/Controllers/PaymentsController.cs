using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe.Data;
using Stripe.Models;

namespace Stripe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public PaymentsController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] StoreProduct product)
        {
            // 1. حفظ الطلب في قاعدة البيانات
            Console.WriteLine("Kareem_Checkout  "); 
            _context.StoreProducts.Add(product);
            await _context.SaveChangesAsync();

            // 2. تهيئة Stripe
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(product.Price * 100), // Stripe يعمل بالسنت
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Name,
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = "http://127.0.0.1:5500/index.html",
                CancelUrl = "https://localhost:5001/cancel",
                Metadata = new Dictionary<string, string>
                {
                    { "OrderId", product.Id.ToString() }
                }
            };

            var service = new SessionService();
            var session = service.Create(options);
            Console.WriteLine(session.Id);
            return Ok(new { url = session.Url });
        }
    }

}
