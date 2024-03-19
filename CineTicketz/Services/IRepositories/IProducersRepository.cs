using CineTicketz.Models;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CineTicketz.Services.IRepositories
{
    public interface IProducersRepository : IGenericRepository<Producer>
    {
        Task<Producer?> GetById(int id);
        Task<IEnumerable<Producer>> GetProducers();
        Task Create(CreateActorFormViewModel model);
        Task<Producer?> Edit(EditActorFormViewModel model);
        bool Delete(int id);

        IEnumerable<SelectListItem> GetSelectListItemsOfProducers();
    }
}
