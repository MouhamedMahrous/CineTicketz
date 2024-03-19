using System.ComponentModel.DataAnnotations;

namespace CineTicketz.Models
{
    public class ActorProducerBaseModel
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string FullName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ProfilePictureURL { get; set; } = string.Empty;
    }
}
