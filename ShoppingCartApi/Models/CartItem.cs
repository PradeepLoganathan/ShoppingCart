using System.ComponentModel.DataAnnotations;

namespace ShoppingCartApi.Models
{
    public class CartItem
    {
        [Key] // Explicitly mark as primary key
        public int CartItemId { get; set; }   // Unique identifier for the cart item
        public int ProductId { get; set; }
        public Product Product { get; set; }  // Include the full product details
        public int Quantity { get; set; }
    }
}
