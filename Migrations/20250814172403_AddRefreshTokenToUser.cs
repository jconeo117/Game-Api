using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonCrawlerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Expires_In",
                table: "Players",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Refresh_Token",
                table: "Players",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expires_In",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Refresh_Token",
                table: "Players");
        }
    }
}
