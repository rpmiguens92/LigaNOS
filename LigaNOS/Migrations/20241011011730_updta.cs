using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaNOS.Migrations
{
    public partial class updta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatId",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatId",
                table: "Clubs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_StatId",
                table: "Matches",
                column: "StatId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_StatId",
                table: "Clubs",
                column: "StatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Stats_StatId",
                table: "Clubs",
                column: "StatId",
                principalTable: "Stats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Stats_StatId",
                table: "Matches",
                column: "StatId",
                principalTable: "Stats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Stats_StatId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Stats_StatId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_StatId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_StatId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "StatId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "StatId",
                table: "Clubs");
        }
    }
}
