namespace CineTicketz.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public Movie Movie { get; set; } = default!;
        public int Amount { get; set; }

        public string ShoppingCartId { get; set; } = string.Empty;
    }
}
