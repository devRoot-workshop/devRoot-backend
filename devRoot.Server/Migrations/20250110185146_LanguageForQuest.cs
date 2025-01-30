using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace devRoot.Server.Migrations
{
    /// <inheritdoc />
    public partial class LanguageForQuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Quests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Quests");
        }
    }
}
