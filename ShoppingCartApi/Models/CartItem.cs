using System.ComponentModel.DataAnnotations;

namespace ShoppingCartApi.Models
{
   public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }   // Unique identifier for the cart item
        public int ProductId { get; set; }
        public string ProductName { get; set; }    // Store product name directly
        public decimal ProductPrice { get; set; }  // Store product price directly
        public int Quantity { get; set; }
    }
}
