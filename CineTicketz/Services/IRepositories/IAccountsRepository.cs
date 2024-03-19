using CineTicketz.Models;

namespace CineTicketz.Services.IRepositories
{
    public interface IAccountsRepository : IGenericRepository<ApplicationUser>
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
    }
}
