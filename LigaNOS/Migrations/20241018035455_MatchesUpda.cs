using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaNOS.Migrations
{
    public partial class MatchesUpda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Clubs_ClubsId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "ClubsId",
                table: "Matches",
                newName: "ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_ClubsId",
                table: "Matches",
                newName: "IX_Matches_ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Clubs_ClubId",
                table: "Matches",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Clubs_ClubId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "ClubId",
                table: "Matches",
                newName: "ClubsId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_ClubId",
                table: "Matches",
                newName: "IX_Matches_ClubsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Clubs_ClubsId",
                table: "Matches",
                column: "ClubsId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
