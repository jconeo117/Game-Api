using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonCrawlerAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dungeons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Dungeon_Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dungeons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Email_Verified = table.Column<bool>(type: "boolean", nullable: false),
                    Last_Login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Is_Active = table.Column<bool>(type: "boolean", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    Gold = table.Column<int>(type: "integer", nullable: false),
                    User = table.Column<string>(type: "text", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Character", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Character_Players_User",
                        column: x => x.User,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DungeonRuns",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    Completion_Time = table.Column<int>(type: "integer", nullable: false),
                    Dungeon_Id = table.Column<string>(type: "text", nullable: false),
                    Character_Id = table.Column<string>(type: "text", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DungeonRuns_Character_Character_Id",
                        column: x => x.Character_Id,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DungeonRuns_Dungeons_Dungeon_Id",
                        column: x => x.Dungeon_Id,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    NumSlots = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Character_UserId",
                        column: x => x.UserId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Item_Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    StatsJson = table.Column<string>(type: "json", nullable: false),
                    Inventario = table.Column<string>(type: "text", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Inventory_Inventario",
                        column: x => x.Inventario,
                        principalTable: "Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items_Shop",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Owner_Id = table.Column<string>(type: "text", nullable: false),
                    Item_Id = table.Column<string>(type: "text", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items_Shop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Shop_Character_Owner_Id",
                        column: x => x.Owner_Id,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Shop_Items_Item_Id",
                        column: x => x.Item_Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Character_User",
                table: "Character",
                column: "User",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DungeonRuns_Character_Id",
                table: "DungeonRuns",
                column: "Character_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DungeonRuns_Dungeon_Id",
                table: "DungeonRuns",
                column: "Dungeon_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_UserId",
                table: "Inventory",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Inventario",
                table: "Items",
                column: "Inventario");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Shop_Item_Id",
                table: "Items_Shop",
                column: "Item_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Shop_Owner_Id",
                table: "Items_Shop",
                column: "Owner_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Email",
                table: "Players",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_Username",
                table: "Players",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DungeonRuns");

            migrationBuilder.DropTable(
                name: "Items_Shop");

            migrationBuilder.DropTable(
                name: "Dungeons");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
