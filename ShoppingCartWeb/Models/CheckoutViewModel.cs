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
    }
}
