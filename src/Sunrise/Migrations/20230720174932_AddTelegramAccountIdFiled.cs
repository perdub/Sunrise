using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sunrise.Migrations
{
    /// <inheritdoc />
    public partial class AddTelegramAccountIdFiled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TelegramAccountId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramAccountId",
                table: "Users");
        }
    }
}
