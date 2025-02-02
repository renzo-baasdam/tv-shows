namespace TvShows.Scraper;

public record TvMazeScraperOptions
{
    public Uri ShowsUri { get; set; } = new(string.Empty, UriKind.Relative);
    public Uri PeopleUri { get; set; } = new(string.Empty, UriKind.Relative);
}