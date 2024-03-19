using System.ComponentModel.DataAnnotations;

namespace CineTicketz.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;

        //Relationships
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
