using CineTicketz.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace CineTicketz.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MovieCategory MovieCategory { get; set; }

        //Relationships
        public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

        public int ProducerId { get; set; }
        public Producer Producer { get; set; } = default!;

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; } = default!;

    }
}
