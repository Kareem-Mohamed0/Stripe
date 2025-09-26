namespace Stripe.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string Status { get; set; } // pending, paid

        public ICollection<CartItem> CartItems { get; set; }
    }
}
