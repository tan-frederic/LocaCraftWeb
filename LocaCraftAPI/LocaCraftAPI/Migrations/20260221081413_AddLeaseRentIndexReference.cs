using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocaCraftAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaseRentIndexReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "RentIndexReference",
                table: "Leases",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentIndexReference",
                table: "Leases");
        }
    }
}
