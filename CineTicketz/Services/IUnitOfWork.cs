using CineTicketz.Services.IRepositories;

namespace CineTicketz.Services
{
    public interface IUnitOfWork
    {
        IActorsRepository Actors { get; }
        IProducersRepository Producers { get; }
        ICinemasRepository Cinemas { get; }
        IMoviesRepository Movies { get; }
        IOrdersRepository Orders { get; }
        IAccountsRepository Users { get; }
    }
}
