namespace CineTicketz.Models
{
    public class Producer : ActorProducerBaseModel
    {
        //Relationships
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
