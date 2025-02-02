namespace TvShows.Api;

public record TvShowsOptions
{
    public Uri ShowsUri { get; set; } = new(string.Empty, UriKind.Relative);
    public Uri PeopleUri { get; set; } = new(string.Empty, UriKind.Relative);
}