using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace devRoot.Server.Migrations
{
    /// <inheritdoc />
    public partial class VoteIdFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoteId",
                table: "Votes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteId",
                table: "Votes");
        }
    }
}
