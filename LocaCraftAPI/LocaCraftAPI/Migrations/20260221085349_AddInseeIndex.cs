using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocaCraftAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddInseeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "RentIndexReference",
                table: "Leases",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "RentIndexReference",
                table: "Leases",
                type: "REAL",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);
        }
    }
}
