namespace CineTicketz.Models.DTOs
{
    public class StoreOrderDto
    {
        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
        public string UserId { get; set; } = string.Empty;
        public string UserEmailAddress { get; set; } = string.Empty;
    }
}
