using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UniqueGroupNameAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name_CategoryId",
                table: "Groups",
                columns: new[] { "Name", "CategoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_Name_CategoryId",
                table: "Groups");
        }
    }
}
