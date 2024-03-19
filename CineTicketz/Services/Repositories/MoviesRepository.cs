using CineTicketz.Data;
using CineTicketz.Data.Enums;
using CineTicketz.Models;
using CineTicketz.Services.IRepositories;
using CineTicketz.Services.MicroServices;
using CineTicketz.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CineTicketz.Services.Repositories
{
    public class MoviesRepository : GenericRepository<Movie>, IMoviesRepository
    {
        private readonly ImageOperations _imageOperations;
        public MoviesRepository(ApplicationDbContext context,
            ImageOperations imageOperations) : base(context)
        {
            _imageOperations = imageOperations;
        }

        public async Task<Movie?> GetById(int id)
        {
            return await _context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Producer)
                .Include(m => m.ActorMovies)
                .ThenInclude(a => a.Actor)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            return await _context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Producer)
                .Include(m => m.ActorMovies)
                .ThenInclude(a => a.Actor)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Create(CreateMovieFormViewModel model)
        {
            var coverName = await _imageOperations.Create(model.FilmPoster);

            var movie = new Movie()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                MovieCategory = (MovieCategory)Enum.Parse(typeof(MovieCategory), model.MovieCategory),
                ImageURL = coverName,
                ActorMovies = model.SelectedActors.Select(a => new ActorMovie { ActorId = a }).ToList(),
                ProducerId = model.ProducerId,
                CinemaId = model.CinemaId
            };

            _context.Add(movie);
            _context.SaveChanges();
        }

        public async Task<Movie?> Edit(EditMovieFormViewModel model)
        {
            var movie = _context.Movies
                .Include(m => m.ActorMovies)
                .SingleOrDefault(m => m.Id == model.Id);

            if (movie is null)
                return null;

            var hasUpdatePoster = model.FilmPoster is not null;
            var oldPoster = movie.ImageURL;

            movie.Name = model.Name;
            movie.Description = model.Description;
            movie.Price = model.Price;
            movie.StartDate = model.StartDate;
            movie.EndDate = model.EndDate;
            movie.MovieCategory = (MovieCategory)Enum.Parse(typeof(MovieCategory), model.MovieCategory);
            movie.ActorMovies = model.SelectedActors.Select(a => new ActorMovie { ActorId = a }).ToList();
            movie.ProducerId = model.ProducerId;
            movie.CinemaId = model.CinemaId;

            if (hasUpdatePoster)
            {
                var newPoster = await _imageOperations.Create(model.FilmPoster);
                movie.ImageURL = newPoster;
            }

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                if (hasUpdatePoster)
                {
                    _imageOperations.Delete(oldPoster);
                }
                return movie;
            }
            else
            {
                _imageOperations.Delete(movie.ImageURL);
                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var movie = _context.Movies.Find(id);

            if (movie is null)
                return isDeleted;

            _context.Remove(movie);

            var affectedRows = _context.SaveChanges();

            if (affectedRows > 0)
            {
                _imageOperations.Delete(movie.ImageURL);
                isDeleted = true;
            }

            return isDeleted;
        }
    }
   
                
}
