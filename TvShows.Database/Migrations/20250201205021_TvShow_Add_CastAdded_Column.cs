using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvShows.Database.Migrations
{
    /// <inheritdoc />
    public partial class TvShow_Add_CastAdded_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CastAdded",
                table: "TvShows",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CastAdded",
                table: "TvShows");
        }
    }
}
