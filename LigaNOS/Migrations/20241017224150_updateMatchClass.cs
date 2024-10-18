using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaNOS.Migrations
{
    public partial class updateMatchClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubsId",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ClubsId",
                table: "Matches",
                column: "ClubsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Clubs_ClubsId",
                table: "Matches",
                column: "ClubsId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Clubs_ClubsId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_ClubsId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ClubsId",
                table: "Matches");
        }
    }
}
