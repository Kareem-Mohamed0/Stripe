using Microsoft.AspNetCore.Identity;
using Stripe.Issuing;

namespace Stripe.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}
