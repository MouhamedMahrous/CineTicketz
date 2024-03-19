using CineTicketz.Data;
using CineTicketz.Models;
using CineTicketz.Services.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CineTicketz.Services.Repositories
{
    public class AccountsRepository : GenericRepository<ApplicationUser>, IAccountsRepository
    {
        public AccountsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _context.Users
                .AsNoTracking() 
                .ToListAsync();  
        }
    }
}
