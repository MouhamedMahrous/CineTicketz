using CineTicketz.Models;
using CineTicketz.ViewModels;

namespace CineTicketz.Services.IRepositories
{
    public interface IMoviesRepository : IGenericRepository<Movie>
    {
        Task<Movie?> GetById(int id);
        Task<IEnumerable<Movie>> GetMovies();
        Task Create(CreateMovieFormViewModel model);
        Task<Movie?> Edit(EditMovieFormViewModel model);
        bool Delete(int id);
    }
}
