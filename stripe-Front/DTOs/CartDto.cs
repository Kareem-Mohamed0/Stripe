using Stripe.Models;

namespace Stripe.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public ICollection<CartItemsDto> CartItems { get; set; }
    }
}
