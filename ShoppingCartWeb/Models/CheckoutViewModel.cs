using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartWeb.Models
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalPrice { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        // Payment Fields
        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card number must be 16 digits.")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Invalid expiry date format.")]
        public string CardExpiry { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV must be 3 digits.")]
        public string CardCVV { get; set; }
    }
}
