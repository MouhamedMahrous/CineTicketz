using CineTicketz.Data;
using CineTicketz.Models;
using CineTicketz.Models.DTOs;
using CineTicketz.Services.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CineTicketz.Services.Repositories
{
    public class OrdersRepository : GenericRepository<Order>, IOrdersRepository
    {
        public OrdersRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAndRole(string userId, string userRole)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Movie)
                .ToListAsync();

            if (userRole != "Admin")
            {
                orders = orders.Where(o => o.UserId == userId).ToList();
            }

            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Movie)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return orders;
        }

        public async Task StoreOrder(StoreOrderDto storeOrderDto)
        {
            var order = new Order()
            {
                UserId = storeOrderDto.UserId,
                Email = storeOrderDto.UserEmailAddress,
            };

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach(var item in storeOrderDto.ShoppingCartItems)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    Price = item.Movie.Price * item.Amount,
                    MovieId = item.Movie.Id,
                    OrderId = order.Id
                };

                await _context.AddAsync(orderItem);
            }

            await _context.SaveChangesAsync();
        }
    }
}
