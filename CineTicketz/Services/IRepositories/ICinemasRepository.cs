using CineTicketz.Models;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CineTicketz.Services.IRepositories
{
    public interface ICinemasRepository : IGenericRepository<Cinema>
    {
        Task<Cinema?> GetById(int id);
        Task<IEnumerable<Cinema>> GetCinemas();
        Task Create(CreateCinemaFormViewModel model);
        Task<Cinema?> Edit(EditCinemaFormViewModel model);
        bool Delete(int id);

        IEnumerable<SelectListItem> GetSelectListItemsOfCinemas();
    }
}
