using CineTicketz.Data;
using CineTicketz.Models;
using CineTicketz.Services.IRepositories;
using CineTicketz.Services.MicroServices;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CineTicketz.Services.Repositories
{
    public class ActorsRepository : GenericRepository<Actor>, IActorsRepository
    {
        private readonly ImageOperations _imageOperations;
        public ActorsRepository(ApplicationDbContext context, 
            ImageOperations imageOperations) : base(context)
        {
            _imageOperations = imageOperations;
        }

        public async Task<Actor?> GetById(int id)
        {
            return await _context.Actors
                           .AsNoTracking()
                           .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Actor>> GetActors()
        {
            return await _context.Actors
                           .AsNoTracking()
                           .ToListAsync();
        }

        public async Task Create(CreateActorFormViewModel model)
        {
            var coverName = await _imageOperations.Create(model.ProfilePicture);

            var actor = new Actor()
            {
                FullName = model.FullName,
                Bio = model.Bio,
                ProfilePictureURL = coverName
            };

            _context.Add(actor);
            _context.SaveChanges(); 
        }

        public async Task<Actor?> Edit(EditActorFormViewModel model)
        {
            var actor = _context.Actors.Find(model.Id);

            if (actor is null)
                return null;

            var hasUpdateProfilePecture = model.ProfilePicture is not null;
            var oldProfilePectureURL = actor.ProfilePictureURL;

            actor.FullName = model.FullName;
            actor.Bio = model.Bio;

            if(hasUpdateProfilePecture)
            {
                var newProfilepectureName = await _imageOperations.Create(model.ProfilePicture);
                actor.ProfilePictureURL = newProfilepectureName;
            }

            var effectedRows = _context.SaveChanges();

            if(effectedRows > 0)
            {
                if (hasUpdateProfilePecture)
                {
                    _imageOperations.Delete(oldProfilePectureURL);
                }
                return actor;
            }
            else
            {
                _imageOperations.Delete(actor.ProfilePictureURL); 
                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var actor = _context.Actors.Find(id);

            if (actor is null)
                return isDeleted;

            _context.Remove(actor);

            var effectedRows = _context.SaveChanges();

            if(effectedRows > 0)
            {
                isDeleted = true;
                _imageOperations.Delete(actor.ProfilePictureURL);
            }

            return isDeleted;
        }

        public IEnumerable<SelectListItem> GetSelectListItemsOfActors()
        {
            return _context.Actors
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FullName })
                .OrderBy(c => c.Text)
                .AsNoTracking()
                .ToList();
        }
    }
}
