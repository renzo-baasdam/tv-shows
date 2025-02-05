using Microsoft.EntityFrameworkCore;
using TvShows.Database;
using Db = TvShows.Database.Models;

namespace TvShows.Api.Endpoints;

internal class TvShowsService(TvShowsDbContext db)
{
    private const int PageSize = 100;
    
    public async Task<Responses.TvShow[]> GetTvShows(int page)
    {
        var pageStart = page * PageSize;
        var pageEnd = (page + 1) * PageSize; // Non-inclusive
        var dbModels = await db.TvShows
            .Include(show => show.Cast.OrderByDescending(person => person.Birthday ?? DateOnly.MinValue))
            .Where(show => show.Id >= pageStart && show.Id < pageEnd)
            .OrderBy(show => show.Id)
            .ToListAsync();
        return dbModels.Count > 0
            ? dbModels.Select(MapToResponseModel).ToArray()
            : [];
    }
    
    private static Responses.TvShow MapToResponseModel(Db.TvShow tvShow) =>
        new()
        {
            Id = tvShow.Id,
            Name = tvShow.Name,
            Cast = tvShow.Cast.Select(MapToResponseModel).ToArray()
        };
    
    private static Responses.Person MapToResponseModel(Db.Person person) =>
        new()
        {
            Id = person.Id,
            Name = person.Name,
            Birthday = person.Birthday
        };
}