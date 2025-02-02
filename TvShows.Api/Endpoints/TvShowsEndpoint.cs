namespace TvShows.Api.Endpoints;

internal static class TvShowsEndpoint
{
    public static void AddTvShowsEndpoint(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.Configure<TvShowsOptions>(configuration.GetSection("TvShowsOptions"));
        services.AddScoped<TvShowsService>();
    }
    
    public static void UseTvShowsEndpoint(this WebApplication app)
    {
        app.MapGet("/shows/{page:int}", GetTvShows);
    }
    
    private static async Task<IResult> GetTvShows(int page, TvShowsService service)
    {
        var shows = await service.GetTvShows(page);
        return shows.Length > 0
            ? Results.Ok(shows)
            : Results.NotFound();
    }
}