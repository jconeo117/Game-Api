using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonCrawlerAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifyItemModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_System_Item",
                table: "Items",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_System_Item",
                table: "Items");
        }
    }
}
