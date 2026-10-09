using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeTypeConnector.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddComputerIdToSecurityTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComputerId",
                table: "SecurityTokens",
                type: "TEXT",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityTokens_ComputerId",
                table: "SecurityTokens",
                column: "ComputerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SecurityTokens_ComputerId",
                table: "SecurityTokens");

            migrationBuilder.DropColumn(
                name: "ComputerId",
                table: "SecurityTokens");
        }
    }
}
