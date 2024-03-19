using CineTicketz.Attributes;
using CineTicketz.Settings;
using System.ComponentModel.DataAnnotations;

namespace CineTicketz.ViewModels
{
    public class EditCinemaFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? CurrentLogo { get; set; }

        [AllowedExtensions(FileSettings.AllowedExtensions)]
        public IFormFile? Logo { get; set; } = default!;
    }
}
