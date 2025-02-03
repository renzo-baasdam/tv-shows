using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using TvShows.Database;
using Db = TvShows.Database.Models;

namespace TvShows.Scraper;

public class TvMazeScraper(
    HttpClient client,
    IServiceScopeFactory scopeFactory,
    IOptions<TvMazeScraperOptions> options) : BackgroundService
{
    private const int ShowsPageSize = 250;
    private const int PeoplePageSize = 1000;
    private const int RateLimitCalls = 20;
    private readonly TimeSpan _rateLimitTime = TimeSpan.FromSeconds(10);
    
    // Stateful variables as BackgroundService is resolved as a singleton
    private readonly ConcurrentQueue<DateTime> _recentRequests = [];
    
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Cannot resolve scoped DbContext into a singleton IHostedService
        using var scope = scopeFactory.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<TvShowsDbContext>();
        var showsUpToDate = false;
        var peopleUpToDate = false;
        var castUpToDate = false;
        // Prioritize getting all tv show data -> then people data -> then link the cast
        while (true)
        {
            if (!showsUpToDate) showsUpToDate = await GetTvShows(stoppingToken, dbContext);
            else if (!peopleUpToDate) peopleUpToDate = await GetPeople(stoppingToken, dbContext);
            else if (!castUpToDate) castUpToDate = await GetCast(stoppingToken, dbContext);
            else return;
        }
    }
    
    private async Task<bool> GetTvShows(CancellationToken ct, TvShowsDbContext dbContext)
    {
        var page = dbContext.TvShows.Any()
            ? dbContext.TvShows.Max(p => p.Id) / ShowsPageSize + 1
            : 0;
        var result = await GetFromExternalApi<External.TvShow>(options.Value.ShowsUri + $"?page={page}", ct);
        if (result.ReachedEnd) return true;
        foreach (var show in result.Value)
        {
            var dbModel = MapToDbModel(show);
            await dbContext.TvShows.AddAsync(dbModel, ct);
        }
        await dbContext.SaveChangesAsync(ct);
        return false;
    }
    
    private async Task<bool> GetPeople(CancellationToken ct, TvShowsDbContext dbContext)
    {
        var page = dbContext.People.Any()
            ? dbContext.People.Max(p => p.Id) / PeoplePageSize + 1
            : 0;
        var result = await GetFromExternalApi<External.Person>(options.Value.PeopleUri + $"?page={page}", ct);
        if (result.ReachedEnd) return true;
        foreach (var person in result.Value)
        {
            var dbPerson = MapToDbModel(person);
            await dbContext.People.AddAsync(dbPerson, ct);
        }
        await dbContext.SaveChangesAsync(ct);
        return false;
    }
    
    private async Task<bool> GetCast(CancellationToken ct, TvShowsDbContext dbContext)
    {
        var show = dbContext.TvShows.FirstOrDefault(show => !show.CastAdded);
        if (show is null) return true;
        var result = await GetFromExternalApi<External.CastItem>(options.Value.ShowsUri + $"/{show.Id}/cast", ct);
        if (result.ReachedEnd) return true;
        
        foreach (var castItem in result.Value)
        {
            var person = await dbContext.People.FindAsync(castItem.Person.Id);
            if (person is not null) show.Cast.Add(person);
        }
        show.CastAdded = true;
        dbContext.Update(show);
        await dbContext.SaveChangesAsync(ct);
        return false;
    }
    
    private async Task<Result<T>> GetFromExternalApi<T>(string url, CancellationToken ct)
    {
        await Task.Delay(MsDelayToEnsureRateLimit(), ct);
        EnqueueRateLimit(DateTime.Now);
        var response = await Retry(
            () => client.GetAsync(url, ct),
            response => !(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound),
            5,
            TimeSpan.FromSeconds(5)
        );
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new Result<T>(true, []);
        }
        if (!response.IsSuccessStatusCode) throw new HttpRequestException($"Received {response.StatusCode} status code.");
        
        var content = await response.Content.ReadAsStringAsync(ct);
        var result = JsonSerializer.Deserialize<List<T>>(content, _serializerOptions);
        
        return new Result<T>(false, result ?? []);
    }
    
    private static async Task<T> Retry<T>(Func<Task<T>> func, Func<T, bool> when, int times, TimeSpan delay) where T : class
    {
        var result = default(T);
        for (int i = 0; i < times; ++i)
        {
            try
            {
                result = await func();
                if (!when(result)) return result;
                await Task.Delay(delay);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Exception occurred with message: {ex.Message}");
            }
        }
        return result!;
    }
    
    private void EnqueueRateLimit(DateTime time)
    {
        if (_recentRequests.Count == RateLimitCalls)
            _recentRequests.TryDequeue(out _);
        _recentRequests.Enqueue(time);
    }
    
    private int MsDelayToEnsureRateLimit()
    {
        if (_recentRequests.Count < RateLimitCalls) return 0;
        _recentRequests.TryPeek(out var firstRequestWithinRateLimitWindow);
        // If above the rate limit, add an extra 100ms for good measure
        var timeout = Math.Max(
            0,
            (firstRequestWithinRateLimitWindow + _rateLimitTime - DateTime.Now).Milliseconds + 500);
        return timeout;
    }
    
    private record Result<T>(bool ReachedEnd, List<T> Value);
    
    private static Db.TvShow MapToDbModel(External.TvShow tvShow) =>
        new()
        {
            Id = tvShow.Id,
            Name = tvShow.Name,
            Cast = []
        };
    
    private static Db.Person MapToDbModel(External.Person person) =>
        new()
        {
            Id = person.Id,
            Name = person.Name,
            Birthday = person.BirthDay,
            TvShows = [],
        };
}