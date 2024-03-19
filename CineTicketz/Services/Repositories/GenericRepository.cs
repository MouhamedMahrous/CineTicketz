using CineTicketz.Data;
using CineTicketz.Services.IRepositories;

namespace CineTicketz.Services.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
