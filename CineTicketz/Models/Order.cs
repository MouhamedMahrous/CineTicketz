namespace CineTicketz.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
