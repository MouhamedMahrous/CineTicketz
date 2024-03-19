using CineTicketz.Attributes;
using CineTicketz.Settings;
using System.ComponentModel.DataAnnotations;

namespace CineTicketz.ViewModels
{
    public class CreateCinemaFormViewModel
    {
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [AllowedExtensions(FileSettings.AllowedExtensions)]
        public IFormFile Logo { get; set; } = default!;
    }
}
