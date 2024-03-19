using System.ComponentModel.DataAnnotations;

namespace CineTicketz.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name ="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
