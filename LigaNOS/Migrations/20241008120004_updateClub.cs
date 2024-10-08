using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaNOS.Migrations
{
    public partial class updateClub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Matches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stadium",
                table: "Clubs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_UserId",
                table: "Matches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_UserId",
                table: "Matches",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_UserId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_UserId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Stadium",
                table: "Clubs");
        }
    }
}
