using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace devRoot.Server.Migrations
{
    /// <inheritdoc />
    public partial class NewQuestFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Quests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Console",
                table: "Quests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Console",
                table: "Quests");
        }
    }
}
