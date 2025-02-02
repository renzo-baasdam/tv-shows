namespace TvShows.Database.Models;

public record TvShow {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool CastAdded { get; set; }
    public List<Person> Cast {get; set; } = [];
}