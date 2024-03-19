using CineTicketz.Data;
using CineTicketz.Models;
using CineTicketz.Services.IRepositories;
using CineTicketz.Services.MicroServices;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CineTicketz.Services.Repositories
{
    public class CinemasRepository : GenericRepository<Cinema>, ICinemasRepository
    {
        private readonly ImageOperations _imageOperations;
        public CinemasRepository(ApplicationDbContext context, 
            ImageOperations imageOperations) : base(context)
        {
            _imageOperations = imageOperations;
        }

        public async Task<Cinema?> GetById(int id)
        {
            return await _context.Cinemas
                           .AsNoTracking()
                           .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Cinema>> GetCinemas()
        {
            return await _context.Cinemas
                           .AsNoTracking()
                           .ToListAsync();
        }

        public async Task Create(CreateCinemaFormViewModel model)
        {
            var logoName = await _imageOperations.Create(model.Logo);

            var cinema = new Cinema()
            {
                Name = model.Name,
                Description = model.Description,
                Logo = logoName
            };

            _context.Add(cinema);
            _context.SaveChanges();
        }

        public async Task<Cinema?> Edit(EditCinemaFormViewModel model)
        {
            var cinema = _context.Cinemas.Find(model.Id);

            if (cinema is null)
                return null;

            var hasUpdateLogo = model.Logo is not null;
            var oldLogo = cinema.Logo;

            cinema.Name = model.Name;
            cinema.Description = model.Description;

            if (hasUpdateLogo)
            {
                var newLogo = await _imageOperations.Create(model.Logo);
                cinema.Logo = newLogo;
            }

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                if (hasUpdateLogo)
                {
                    _imageOperations.Delete(oldLogo);
                }
                return cinema;
            }
            else
            {
                _imageOperations.Delete(cinema.Logo);
                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var cinema = _context.Cinemas.Find(id);

            if (cinema is null)
                return isDeleted;

            _context.Remove(cinema);

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
                _imageOperations.Delete(cinema.Logo);
            }

            return isDeleted;
        }

        public IEnumerable<SelectListItem> GetSelectListItemsOfCinemas()
        {
            return _context.Cinemas
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                .OrderBy(c => c.Text)
                .AsNoTracking()
                .ToList();
        }
    }
}
