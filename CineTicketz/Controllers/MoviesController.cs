using CineTicketz.Data.Enums;
using CineTicketz.Models;
using CineTicketz.Services;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CineTicketz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MoviesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var movies = await _unitOfWork.Movies.GetMovies();

            return View(movies);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var movies = await _unitOfWork.Movies.GetMovies();
            IEnumerable<Movie> filteredMovies = Enumerable.Empty<Movie>();

            if (!string.IsNullOrEmpty(searchString))
            {
                filteredMovies = movies
                    .Where(m => m.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) || m.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(filteredMovies);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _unitOfWork.Movies.GetById(id);

            if(movie is null)
                return NotFound();

            return View(movie);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateMovieFormViewModel()
            {
                Categories = Enum.GetNames(typeof(MovieCategory))
                            .Select(c => new SelectListItem { Value = c, Text = c }),

                Actors = _unitOfWork.Actors.GetSelectListItemsOfActors(),
                Producers = _unitOfWork.Producers.GetSelectListItemsOfProducers(),
                Cinemas = _unitOfWork.Cinemas.GetSelectListItemsOfCinemas()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = Enum.GetNames(typeof(MovieCategory))
                           .Select(c => new SelectListItem { Value = c, Text = c });

                model.Actors = _unitOfWork.Actors.GetSelectListItemsOfActors();
                model.Producers = _unitOfWork.Producers.GetSelectListItemsOfProducers();
                model.Cinemas = _unitOfWork.Cinemas.GetSelectListItemsOfCinemas();

                return View(model);
            }

            await _unitOfWork.Movies.Create(model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _unitOfWork.Movies.GetById(id);

            if (movie is null)
                return NotFound();

            var viewModel = new EditMovieFormViewModel()
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                StartDate = movie.StartDate,
                EndDate = movie.EndDate,
                MovieCategory = movie.MovieCategory.ToString(),
                Categories = Enum.GetNames(typeof(MovieCategory))
                           .Select(c => new SelectListItem { Value = c, Text = c }),

                SelectedActors = movie.ActorMovies.Select(a => a.ActorId).ToList(),
                Actors = _unitOfWork.Actors.GetSelectListItemsOfActors(),
                ProducerId = movie.ProducerId,
                Producers = _unitOfWork.Producers.GetSelectListItemsOfProducers(),
                CinemaId = movie.CinemaId,
                Cinemas = _unitOfWork.Cinemas.GetSelectListItemsOfCinemas(),
                CurrentPosterName = movie.ImageURL
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditMovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = Enum.GetNames(typeof(MovieCategory))
                            .Select(c => new SelectListItem { Value = c, Text = c });

                model.Actors = _unitOfWork.Actors.GetSelectListItemsOfActors();
                model.Producers = _unitOfWork.Producers.GetSelectListItemsOfProducers();
                model.Cinemas = _unitOfWork.Cinemas.GetSelectListItemsOfCinemas();

                return View(model);
            }

            var movie = await _unitOfWork.Movies.Edit(model);

            if (model is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var isDeleted = _unitOfWork.Movies.Delete(id);

            return isDeleted ? RedirectToAction(nameof(Index)) : BadRequest();
        }
    }
}
