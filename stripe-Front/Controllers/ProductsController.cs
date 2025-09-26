using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Data;
using Stripe.Models;


namespace Stripe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // رفع صورة المنتج
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/products/{fileName}";
            return Ok(new { Url = imageUrl });
        }

        // إضافة منتج جديد
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] StoreProduct storeProduct)
        {
            _context.StoreProducts.Add(storeProduct);
            await _context.SaveChangesAsync();
            return Ok(storeProduct);
        }

        // جلب كل المنتجات
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.StoreProducts.ToListAsync();
            return Ok(products);
        }
    }

}
