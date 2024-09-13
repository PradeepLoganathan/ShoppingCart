namespace ShoppingCartWeb.Models
{
    public class CartItem
    {
        public int Id { get; set; }  // This is the unique ID for the cart item
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
