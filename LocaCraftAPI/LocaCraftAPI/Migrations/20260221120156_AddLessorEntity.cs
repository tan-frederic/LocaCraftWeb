using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocaCraftAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLessorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessorId",
                table: "Leases",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lessors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leases_LessorId",
                table: "Leases",
                column: "LessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Lessors_LessorId",
                table: "Leases",
                column: "LessorId",
                principalTable: "Lessors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Lessors_LessorId",
                table: "Leases");

            migrationBuilder.DropTable(
                name: "Lessors");

            migrationBuilder.DropIndex(
                name: "IX_Leases_LessorId",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "LessorId",
                table: "Leases");
        }
    }
}
