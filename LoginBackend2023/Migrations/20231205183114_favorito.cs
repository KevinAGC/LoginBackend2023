using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginBackend2023.Migrations
{
    /// <inheritdoc />
    public partial class favorito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Favoritos",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Favoritos_Email_Link",
                table: "Favoritos",
                newName: "IX_Favoritos_UserId_Link");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Favoritos",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_Favoritos_UserId_Link",
                table: "Favoritos",
                newName: "IX_Favoritos_Email_Link");
        }
    }
}
