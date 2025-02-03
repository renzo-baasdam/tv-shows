# Projects
TvShows.Api:
- The endpoint `/shows/{page}` gives a paginated list with at most 100 entries.

TvShows.Database:
- Tables "TvShows" and "People" and a join table for the cast.
- Migration does not happen automatically, see [TvShows.Database/README.md](TvShows.Database/README.md) for useful commands.

TvShows.Scraper:
- Background service to get all TV shows, People and cast from [TvMaze]([TvMaze](http://www.tvmaze.com/api)), in this order.
- The cast can only be retrieved per show, which means for ~80.000 shows and a rate limit of ~20 per 10 seconds, it will take around 11 hours to fill the database with the cast.
