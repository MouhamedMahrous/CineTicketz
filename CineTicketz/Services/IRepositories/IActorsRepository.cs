using CineTicketz.Models;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CineTicketz.Services.IRepositories
{
    public interface IActorsRepository : IGenericRepository<Actor>
    {
        Task<Actor?> GetById(int id);
        Task<IEnumerable<Actor>> GetActors();
        Task Create(CreateActorFormViewModel model);
        Task<Actor?> Edit(EditActorFormViewModel model);
        bool Delete(int id);

        IEnumerable<SelectListItem> GetSelectListItemsOfActors();
    }
}
