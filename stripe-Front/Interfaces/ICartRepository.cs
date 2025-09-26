using Stripe.DTOs;
using Stripe.Models;

namespace Stripe.Interfaces
{
    public interface ICartRepository
    {
        Task<CartDto> GetUserCartAsync(string userId);
        Task AddToCartAsync(string userId, int productId, int quantity);
    }
}
