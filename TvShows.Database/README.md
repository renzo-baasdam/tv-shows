Some useful commands

```
cd ./TvShows.Database
dotnet ef migrations --startup-project ../TvShows.Api/TvShows.Api.csproj add [MigrationName]
dotnet ef database --startup-project ../TvShows.Api/TvShows.Api.csproj update
dotnet ef database --startup-project ../TvShows.Api/TvShows.Api.csproj update 0
dotnet ef migrations --startup-project ../TvShows.Api/TvShows.Api.csproj remove
```
