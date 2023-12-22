using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sunrise.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingToPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: Sunrise.Types.Enums.PostRating.Warning);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Posts");
        }
    }
}
