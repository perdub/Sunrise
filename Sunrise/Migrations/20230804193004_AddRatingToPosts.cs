using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sunrise.Migrations
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
                type: "INTEGER",
                nullable: false,
                defaultValue: (int)Sunrise.Types.Rating.Unset);
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
