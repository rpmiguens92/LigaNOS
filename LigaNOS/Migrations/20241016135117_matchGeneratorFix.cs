using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaNOS.Migrations
{
    public partial class matchGeneratorFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stats_Clubs_AwayClubId",
                table: "Stats");

            migrationBuilder.DropForeignKey(
                name: "FK_Stats_Clubs_HomeClubId",
                table: "Stats");

            migrationBuilder.AddForeignKey(
                name: "FK_Stats_Clubs_AwayClubId",
                table: "Stats",
                column: "AwayClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stats_Clubs_HomeClubId",
                table: "Stats",
                column: "HomeClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stats_Clubs_AwayClubId",
                table: "Stats");

            migrationBuilder.DropForeignKey(
                name: "FK_Stats_Clubs_HomeClubId",
                table: "Stats");

            migrationBuilder.AddForeignKey(
                name: "FK_Stats_Clubs_AwayClubId",
                table: "Stats",
                column: "AwayClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stats_Clubs_HomeClubId",
                table: "Stats",
                column: "HomeClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
