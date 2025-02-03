using Microsoft.EntityFrameworkCore;
using TvShows.Api.Endpoints;
using TvShows.Database;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<TvShowsDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("TvShows")));
builder.Services.AddTvShowsEndpoint(configuration);

var app = builder.Build();
app.MapOpenApi();
app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "v1"));
app.UseTvShowsEndpoint();

await app.RunAsync();