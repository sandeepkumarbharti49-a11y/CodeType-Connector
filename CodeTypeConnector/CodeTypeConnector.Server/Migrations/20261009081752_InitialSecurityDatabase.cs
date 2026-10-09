using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeTypeConnector.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialSecurityDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecurityTokens",
                columns: table => new
                {
                    TokenHash = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RevokedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityTokens", x => x.TokenHash);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SecurityTokens_ExpiresAtUtc",
                table: "SecurityTokens",
                column: "ExpiresAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityTokens_RevokedAtUtc",
                table: "SecurityTokens",
                column: "RevokedAtUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecurityTokens");
        }
    }
}
