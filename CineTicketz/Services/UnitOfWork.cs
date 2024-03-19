using CineTicketz.Data;
using CineTicketz.Services.IRepositories;
using CineTicketz.Services.MicroServices;
using CineTicketz.Services.Repositories;

namespace CineTicketz.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageOperations _imageOperations;
        public IActorsRepository Actors { get; private set; }
        public IProducersRepository Producers { get; private set; }
        public ICinemasRepository Cinemas { get; private set; }
        public IMoviesRepository Movies { get; private set; }
        public IOrdersRepository Orders { get; private set; }
        public IAccountsRepository Users { get; private set; }

        public UnitOfWork(ApplicationDbContext context, ImageOperations imageOperations)
        {
            _imageOperations = imageOperations;
            _context = context;
            Actors = new ActorsRepository(_context, _imageOperations);
            Producers = new ProducersRepository(_context, _imageOperations);
            Cinemas = new CinemasRepository(_context, _imageOperations);
            Movies = new MoviesRepository(_context, _imageOperations);
            Orders = new OrdersRepository(_context);
            Users = new AccountsRepository(_context);
        }
    }
}
