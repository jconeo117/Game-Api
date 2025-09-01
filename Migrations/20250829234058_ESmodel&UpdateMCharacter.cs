using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonCrawlerAPI.Migrations
{
    /// <inheritdoc />
    public partial class ESmodelUpdateMCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseStatsJson",
                table: "Character",
                type: "json",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EquipmentSlots",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CharacterId = table.Column<string>(type: "text", nullable: false),
                    SlotType = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentSlots_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentSlots_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentSlots_CharacterId_SlotType",
                table: "EquipmentSlots",
                columns: new[] { "CharacterId", "SlotType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentSlots_ItemId",
                table: "EquipmentSlots",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentSlots");

            migrationBuilder.DropColumn(
                name: "BaseStatsJson",
                table: "Character");
        }
    }
}
