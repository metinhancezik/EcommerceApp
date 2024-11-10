using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesLayer.Migrations
{
    /// <inheritdoc />
    public partial class upgradeDbDeletedVentorIdFromStatues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatuses_Vendors_VendorId",
                table: "OrderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatuses_VendorId",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "OrderStatuses");

            migrationBuilder.AddColumn<long>(
                name: "VendorsId",
                table: "OrderStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_VendorsId",
                table: "OrderStatuses",
                column: "VendorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatuses_Vendors_VendorsId",
                table: "OrderStatuses",
                column: "VendorsId",
                principalTable: "Vendors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatuses_Vendors_VendorsId",
                table: "OrderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatuses_VendorsId",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "VendorsId",
                table: "OrderStatuses");

            migrationBuilder.AddColumn<long>(
                name: "VendorId",
                table: "OrderStatuses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_VendorId",
                table: "OrderStatuses",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatuses_Vendors_VendorId",
                table: "OrderStatuses",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
