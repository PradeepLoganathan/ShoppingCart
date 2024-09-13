namespace ShoppingCartWeb.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }   // Updated property name
        public int ProductId { get; set; }
        public Product Product { get; set; }  // Full product details
        public int Quantity { get; set; }
    }
}
