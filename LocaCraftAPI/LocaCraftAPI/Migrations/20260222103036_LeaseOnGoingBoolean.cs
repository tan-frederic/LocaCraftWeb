using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocaCraftAPI.Migrations
{
    /// <inheritdoc />
    public partial class LeaseOnGoingBoolean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOngoing",
                table: "Leases",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOngoing",
                table: "Leases");
        }
    }
}
