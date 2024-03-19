using CineTicketz.Services.MicroServices;

namespace CineTicketz.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart { get; set; } = default!;
        public decimal ShoppingCartTotal { get; set; }
    }
}
