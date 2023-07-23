using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sunrise.Migrations
{
    /// <inheritdoc />
    public partial class FixVerifyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Verify_Users_UserId",
                table: "Verify");

            migrationBuilder.DropIndex(
                name: "IX_Verify_UserId",
                table: "Verify");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Verify_UserId",
                table: "Verify",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Verify_Users_UserId",
                table: "Verify",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
