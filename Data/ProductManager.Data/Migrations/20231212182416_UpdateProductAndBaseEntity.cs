using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductAndBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ProductIndex",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProduceDate",
                table: "Products",
                newName: "LastModifiedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "ProductIndex",
                table: "Products",
                columns: new[] { "CreateDate", "ManufacturerEmail" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ProductIndex",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "Products",
                newName: "ProduceDate");

            migrationBuilder.CreateIndex(
                name: "ProductIndex",
                table: "Products",
                columns: new[] { "ProduceDate", "ManufacturerEmail" },
                unique: true);
        }
    }
}
