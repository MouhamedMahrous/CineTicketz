namespace CineTicketz.Models
{
    public class Actor : ActorProducerBaseModel
    {
        //Relationships
        ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();
    }
}
