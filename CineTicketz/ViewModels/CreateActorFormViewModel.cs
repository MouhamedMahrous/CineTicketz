using CineTicketz.Attributes;
using CineTicketz.Settings;
using System.ComponentModel.DataAnnotations;

namespace CineTicketz.ViewModels
{
    public class CreateActorFormViewModel
    {
        [MaxLength(250)]
        [Display(Name ="Name")]
        public string FullName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [Display(Name = "Picture")]
        public IFormFile ProfilePicture { get; set; } = default!;
    }
}
