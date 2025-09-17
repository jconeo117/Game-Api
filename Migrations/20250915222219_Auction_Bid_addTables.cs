using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonCrawlerAPI.Migrations
{
    /// <inheritdoc />
    public partial class Auction_Bid_addTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Inventario",
                table: "Items",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Auction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Item_Id = table.Column<string>(type: "text", nullable: false),
                    Seller_Character = table.Column<string>(type: "text", nullable: false),
                    Starting_Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Buyout_Price = table.Column<decimal>(type: "numeric", nullable: true),
                    Start_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Auction_Status = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auction_Character_Seller_Character",
                        column: x => x.Seller_Character,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auction_Items_Item_Id",
                        column: x => x.Item_Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Auction_Id = table.Column<string>(type: "text", nullable: false),
                    Bidder_Character = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BidTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_Auction_Auction_Id",
                        column: x => x.Auction_Id,
                        principalTable: "Auction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bids_Character_Bidder_Character",
                        column: x => x.Bidder_Character,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auction_Item_Id",
                table: "Auction",
                column: "Item_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Auction_Seller_Character",
                table: "Auction",
                column: "Seller_Character");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_Auction_Id",
                table: "Bids",
                column: "Auction_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_Bidder_Character",
                table: "Bids",
                column: "Bidder_Character");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "Auction");

            migrationBuilder.AlterColumn<string>(
                name: "Inventario",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
