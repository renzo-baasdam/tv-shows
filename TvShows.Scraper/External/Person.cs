namespace TvShows.Scraper.External;

public record Person {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? BirthDay { get; set; } // Can be null apparently
}