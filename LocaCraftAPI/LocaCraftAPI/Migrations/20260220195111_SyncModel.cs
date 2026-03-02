using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocaCraftAPI.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lease_RealEstateAssets_RealEstateAssetId",
                table: "Lease");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaseDocuments_Lease_LeaseId",
                table: "LeaseDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenant_Lease_LeaseId",
                table: "Tenant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenant",
                table: "Tenant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lease",
                table: "Lease");

            migrationBuilder.RenameTable(
                name: "Tenant",
                newName: "Tenants");

            migrationBuilder.RenameTable(
                name: "Lease",
                newName: "Leases");

            migrationBuilder.RenameIndex(
                name: "IX_Tenant_LeaseId",
                table: "Tenants",
                newName: "IX_Tenants_LeaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Lease_RealEstateAssetId",
                table: "Leases",
                newName: "IX_Leases_RealEstateAssetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leases",
                table: "Leases",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseDocuments_Leases_LeaseId",
                table: "LeaseDocuments",
                column: "LeaseId",
                principalTable: "Leases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_RealEstateAssets_RealEstateAssetId",
                table: "Leases",
                column: "RealEstateAssetId",
                principalTable: "RealEstateAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Leases_LeaseId",
                table: "Tenants",
                column: "LeaseId",
                principalTable: "Leases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaseDocuments_Leases_LeaseId",
                table: "LeaseDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Leases_RealEstateAssets_RealEstateAssetId",
                table: "Leases");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Leases_LeaseId",
                table: "Tenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leases",
                table: "Leases");

            migrationBuilder.RenameTable(
                name: "Tenants",
                newName: "Tenant");

            migrationBuilder.RenameTable(
                name: "Leases",
                newName: "Lease");

            migrationBuilder.RenameIndex(
                name: "IX_Tenants_LeaseId",
                table: "Tenant",
                newName: "IX_Tenant_LeaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Leases_RealEstateAssetId",
                table: "Lease",
                newName: "IX_Lease_RealEstateAssetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenant",
                table: "Tenant",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lease",
                table: "Lease",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lease_RealEstateAssets_RealEstateAssetId",
                table: "Lease",
                column: "RealEstateAssetId",
                principalTable: "RealEstateAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseDocuments_Lease_LeaseId",
                table: "LeaseDocuments",
                column: "LeaseId",
                principalTable: "Lease",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_Lease_LeaseId",
                table: "Tenant",
                column: "LeaseId",
                principalTable: "Lease",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
