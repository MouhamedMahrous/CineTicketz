using CineTicketz.Data;
using CineTicketz.Models;
using Microsoft.EntityFrameworkCore;

namespace CineTicketz.Services.MicroServices
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _context;

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(ApplicationDbContext context)
        {
            _context = context;
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<ApplicationDbContext>();

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(context)
            {
                ShoppingCartId = cartId
            };
        }

        public void AddItemToCart(Movie movie)
        {
            var shoppingCartItem = _context.ShoppingCartItems
                .FirstOrDefault(i => (i.Movie.Id == movie.Id) && (i.ShoppingCartId == ShoppingCartId));

            if (shoppingCartItem is null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1,
                };

                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;    
            }


            _context.SaveChanges();
        }

        public void RemoveItemFromCart(Movie movie)
        {
            var shoppingCartItem = _context.ShoppingCartItems
                .FirstOrDefault(i => i.Movie.Id == movie.Id && i.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem is not null)
            {
                if(shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    _context.Remove(shoppingCartItem);
                }
            }

            _context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems
                .Where(i => i.ShoppingCartId == ShoppingCartId)
                .Include(i => i.Movie)
                .ToList());
        }

        public decimal GetShoppingCartToatal()
        {
            var Total = _context.ShoppingCartItems
                .Where(i => i.ShoppingCartId == ShoppingCartId)
                .Select(i => i.Movie.Price * i.Amount)
                .Sum();

            return Total;
        }

        public async Task ClearShoppingCart()
        {
            var items = await _context.ShoppingCartItems
                .Where(i => i.ShoppingCartId == ShoppingCartId)
                .ToListAsync();

            _context.RemoveRange(items);

            ShoppingCartItems = new List<ShoppingCartItem>();

            await _context.SaveChangesAsync();
        }
    }
}
