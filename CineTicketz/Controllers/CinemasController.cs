using CineTicketz.Data;
using CineTicketz.Services;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineTicketz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CinemasController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CinemasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var cinemas = await _unitOfWork.Cinemas.GetCinemas();
            return View(cinemas);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var cinema = await _unitOfWork.Cinemas.GetById(id);

            if (cinema is null)
                return NotFound();

            return View(cinema);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateCinemaFormViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCinemaFormViewModel model)
        {
            if (!ModelState.IsValid) 
            { 
                return View(model);
            }

            await _unitOfWork.Cinemas.Create(model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cinema = await _unitOfWork.Cinemas.GetById(id);

            if(cinema is null)
                return NotFound();

            var viewModel = new EditCinemaFormViewModel()
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Description = cinema.Description,
                CurrentLogo = cinema.Logo
            }; 

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCinemaFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  
            }

            var cinema = await _unitOfWork.Cinemas.Edit(model);

            if(cinema is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var isDeleted = _unitOfWork.Cinemas.Delete(id);

            return isDeleted ? RedirectToAction(nameof(Index)) : BadRequest();
        }

    }
}
