using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sunrise.Migrations
{
    /// <inheritdoc />
    public partial class WaitForReviewFiledAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WaitForReview",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaitForReview",
                table: "Posts");
        }
    }
}
