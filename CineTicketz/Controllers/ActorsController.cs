using CineTicketz.Data;
using CineTicketz.Services;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineTicketz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ActorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        public async Task<IActionResult> Index()
        {
            var actors = await _unitOfWork.Actors.GetActors();

            return View(actors);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var actor = await _unitOfWork.Actors.GetById(id);

            if (actor is null)
                return NotFound();

            return View(actor);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateActorFormViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateActorFormViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            
            await _unitOfWork.Actors.Create(Model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _unitOfWork.Actors.GetById(id);

            if (actor is null)
                return NotFound();

            var viewModel = new EditActorFormViewModel()
            {
                Id = actor.Id,
                FullName = actor.FullName,
                Bio = actor.Bio,
                CurrentProfilePecture = actor.ProfilePictureURL
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditActorFormViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }

            var actor = await _unitOfWork.Actors.Edit(Model);

            if(actor is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var isDeleted = _unitOfWork.Actors.Delete(id);

            return isDeleted? RedirectToAction(nameof(Index)) : BadRequest();
        }

    }
}
