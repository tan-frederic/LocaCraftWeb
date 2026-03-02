using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocaCraftAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCityOnLessorData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Lessors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Lessors");
        }
    }
}
