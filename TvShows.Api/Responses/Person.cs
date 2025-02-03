namespace TvShows.Api.Responses;

public record Person {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? Birthday { get; set; }
}