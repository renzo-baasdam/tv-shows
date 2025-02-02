using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TvShows.Database;
using TvShows.Scraper;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((context, services) =>
{
    services.AddHttpClient();
    services.Configure<TvMazeScraperOptions>(context.Configuration.GetSection("TvMazeScraperOptions"));
    services.AddHostedService<TvMazeScraper>();
    services.AddDbContext<TvShowsDbContext>(o =>
    {
        o.UseNpgsql(context.Configuration.GetConnectionString("TvShows"));
    });
});

var host = builder.Build();
await host.RunAsync();
