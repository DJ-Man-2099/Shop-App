using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class TokenDBCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenBlacklists",
                columns: table => new
                {
                    Token = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenBlacklists", x => x.Token);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenBlacklists");
        }
    }
}
