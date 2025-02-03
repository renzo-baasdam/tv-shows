using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvShows.Database.Migrations
{
    /// <inheritdoc />
    public partial class Change_Birthday_Column_Capitalization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDay",
                table: "People",
                newName: "Birthday");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "People",
                newName: "BirthDay");
        }
    }
}
