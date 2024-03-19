using CineTicketz.Data;
using CineTicketz.Models;
using CineTicketz.Services.IRepositories;
using CineTicketz.Services.MicroServices;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CineTicketz.Services.Repositories
{
    public class ProducersRepository : GenericRepository<Producer>, IProducersRepository
    {
        private readonly ImageOperations _imageOperations;
        public ProducersRepository(ApplicationDbContext context,
            ImageOperations imageOperations) : base(context)
        {
            _imageOperations = imageOperations;
        }

        public async Task<Producer?> GetById(int id)
        {
            return await _context.Producers
                           .AsNoTracking()
                           .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Producer>> GetProducers()
        {
            return await _context.Producers
                           .AsNoTracking()
                           .ToListAsync();
        }

        public async Task Create(CreateActorFormViewModel model)
        {
            var coverName = await _imageOperations.Create(model.ProfilePicture);

            var producer = new Producer()
            {
                FullName = model.FullName,
                Bio = model.Bio,
                ProfilePictureURL = coverName
            };

            _context.Add(producer);
            _context.SaveChanges();
        }

        public async Task<Producer?> Edit(EditActorFormViewModel model)
        {
            var producer = _context.Producers.Find(model.Id);

            if (producer is null)
                return null;

            var hasUpdateProfilePecture = model.ProfilePicture is not null;
            var oldProfilePectureURL = producer.ProfilePictureURL;

            producer.FullName = model.FullName;
            producer.Bio = model.Bio;

            if (hasUpdateProfilePecture)
            {
                var newProfilepectureName = await _imageOperations.Create(model.ProfilePicture);
                producer.ProfilePictureURL = newProfilepectureName;
            }

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                if (hasUpdateProfilePecture)
                {
                    _imageOperations.Delete(oldProfilePectureURL);
                }
                return producer;
            }
            else
            {
                _imageOperations.Delete(producer.ProfilePictureURL);
                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var producer = _context.Producers.Find(id);

            if (producer is null)
                return isDeleted;

            _context.Remove(producer);

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
                _imageOperations.Delete(producer.ProfilePictureURL);
            }

            return isDeleted;
        }

        public IEnumerable<SelectListItem> GetSelectListItemsOfProducers()
        {
            return _context.Producers
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FullName })
                .OrderBy(c => c.Text)
                .AsNoTracking()
                .ToList();
        }
    }
}
