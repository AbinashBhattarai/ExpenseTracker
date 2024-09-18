using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class EntityRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Transaction",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Category",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AppUserId",
                table: "Transaction",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_AppUserId",
                table: "Category",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_AspNetUsers_AppUserId",
                table: "Category",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AspNetUsers_AppUserId",
                table: "Transaction",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_AspNetUsers_AppUserId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AspNetUsers_AppUserId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_AppUserId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Category_AppUserId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Category");
        }
    }
}
