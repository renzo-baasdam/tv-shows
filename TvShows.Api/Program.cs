using Microsoft.EntityFrameworkCore;
using TvShows.Api;
using TvShows.Database;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<TvShowsDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("TvShows")));
builder.Services.AddTvShowsEndpoint(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

//app.UseHttpsRedirection();
app.UseTvShowsEndpoint();

await app.RunAsync();