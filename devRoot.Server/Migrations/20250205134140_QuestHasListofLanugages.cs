using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace devRoot.Server.Migrations
{
    /// <inheritdoc />
    public partial class QuestHasListofLanugages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Quests");

            migrationBuilder.AddColumn<int[]>(
                name: "Languages",
                table: "Quests",
                type: "integer[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Quests");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Quests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
