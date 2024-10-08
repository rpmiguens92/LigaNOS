using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaNOS.Migrations
{
    public partial class addClubSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Clubs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_UserId",
                table: "Clubs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_AspNetUsers_UserId",
                table: "Clubs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_AspNetUsers_UserId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_UserId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Clubs");
        }
    }
}
