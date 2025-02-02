namespace TvShows.Api.Responses;

public record TvShow {
    public int Id { get; set; }
    public string Name { get; set; }
    public Person[] Cast {get; set; } = [];
}