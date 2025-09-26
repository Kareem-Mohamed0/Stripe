using Microsoft.EntityFrameworkCore;
using Stripe.Data;
using Stripe.DTOs;
using Stripe.Interfaces;
using Stripe.Models;

namespace Stripe.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        // رجع الـ Cart كـ DTO
        public async Task<CartDto> GetUserCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return null;

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Status = cart.Status,
                CartItems = cart.CartItems.Select(i => new CartItemsDto
                {
                    Id = i.Id,
                    ProductName = i.Product.Name,
                    UnitPrice = i.Product.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        // إضافة منتج للـ Cart
        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            // جلب الـ Cart Entity من DB
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Status = "Pending",
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }

            // جلب المنتج من DB
            var product = await _context.StoreProducts.FindAsync(productId);
            if (product == null) throw new Exception("Product not found");

            // التحقق إذا العنصر موجود في الـ Cart
            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
