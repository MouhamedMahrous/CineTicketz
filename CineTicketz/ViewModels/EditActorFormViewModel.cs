using CineTicketz.Attributes;
using CineTicketz.Settings;
using System.ComponentModel.DataAnnotations;

namespace CineTicketz.ViewModels
{
    public class EditActorFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(250)]
        [Display(Name = "Name")]
        public string FullName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        public string? CurrentProfilePecture { get; set; } 

        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [Display(Name = "Picture")]
        public IFormFile? ProfilePicture { get; set; } = default!;
    }
}
