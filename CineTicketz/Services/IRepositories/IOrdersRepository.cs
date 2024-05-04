using CineTicketz.Models;
using CineTicketz.Models.DTOs;
using CineTicketz.ViewModels;

namespace CineTicketz.Services.IRepositories
{
    public interface IOrdersRepository : IGenericRepository<Order>
    {
        Task StoreOrder(StoreOrderDto storeOrderDto);
        Task <IEnumerable<Order>> GetOrdersByUserIdAndRole(string userId, string userRole);
        Task <IEnumerable<Order>> GetOrdersByUserId(string userId);
    }
}
