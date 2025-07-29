using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationToFilm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Films");
        }
    }
}
