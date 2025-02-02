using Microsoft.EntityFrameworkCore;
using TvShows.Database.Models;

namespace TvShows.Database;

public class TvShowsDbContext(DbContextOptions<TvShowsDbContext> options) : DbContext(options)
{
    public DbSet<TvShow> TvShows { get; init; }
    public DbSet<Person> People { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TvShow>()
            .ToTable("TvShows");
        
        modelBuilder.Entity<Person>()
            .ToTable("People");
    }
}