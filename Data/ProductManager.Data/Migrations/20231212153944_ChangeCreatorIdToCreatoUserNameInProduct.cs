using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCreatorIdToCreatoUserNameInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Products",
                newName: "CreatorUserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorUserName",
                table: "Products",
                newName: "CreatorId");
        }
    }
}
