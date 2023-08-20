using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sunrise.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWaitForReviewToStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WaitForReview",
                table: "Posts",
                newName: "Status");

            /*
            migrationBuilder.AddColumn<string>(
                name: "Sha1",
                table: "Files",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
                //*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sha1",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Posts",
                newName: "WaitForReview");
        }
    }
}
