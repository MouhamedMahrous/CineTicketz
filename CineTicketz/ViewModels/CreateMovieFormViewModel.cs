using CineTicketz.Attributes;
using CineTicketz.Data.Enums;
using CineTicketz.Models;
using CineTicketz.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CineTicketz.ViewModels
{
    public class CreateMovieFormViewModel
    {
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name="Category")]
        public string MovieCategory { get; set; } = string.Empty;
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();

        [Display(Name = "Actors")]
        public List<int> SelectedActors { get; set; } = default!;
        public IEnumerable<SelectListItem> Actors { get; set; } = Enumerable.Empty<SelectListItem>();

        [Display(Name="Producer")]
        public int ProducerId { get; set; }
        public IEnumerable<SelectListItem> Producers { get; set; } = Enumerable.Empty<SelectListItem>();

        [Display(Name="Cinema")]
        public int CinemaId { get; set; }
        public IEnumerable<SelectListItem> Cinemas { get; set; } = Enumerable.Empty<SelectListItem>();

        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [Display(Name="Poster")]
        public IFormFile FilmPoster { get; set; } = default!;
    }
}
