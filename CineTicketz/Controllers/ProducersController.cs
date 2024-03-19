using CineTicketz.Data;
using CineTicketz.Services;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineTicketz.Controllers
{
    [Authorize]
    public class ProducersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProducersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var producers = await _unitOfWork.Producers.GetProducers();

            return View(producers);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var producer = await _unitOfWork.Producers.GetById(id);

            if (producer is null)
                return NotFound();

            return View(producer);
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

            await _unitOfWork.Producers.Create(Model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var producer = await _unitOfWork.Producers.GetById(id);

            if (producer is null)
                return NotFound();

            var viewModel = new EditActorFormViewModel()
            {
                Id = producer.Id,
                FullName = producer.FullName,
                Bio = producer.Bio,
                CurrentProfilePecture = producer.ProfilePictureURL
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

            var producer = await _unitOfWork.Producers.Edit(Model);

            if (producer is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var isDeleted = _unitOfWork.Producers.Delete(id);

            return isDeleted ? RedirectToAction(nameof(Index)) : BadRequest();
        }

    }
}
