using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaNOS.Migrations
{
    public partial class addClubs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Club_ClubId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Club",
                table: "Club");

            migrationBuilder.RenameTable(
                name: "Club",
                newName: "Clubs");

            migrationBuilder.AlterColumn<string>(
                name: "ImageFile",
                table: "Clubs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clubs",
                table: "Clubs",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Clubs_ClubId",
                table: "Players",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Clubs_ClubId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clubs",
                table: "Clubs");

            migrationBuilder.RenameTable(
                name: "Clubs",
                newName: "Club");

            migrationBuilder.AlterColumn<string>(
                name: "ImageFile",
                table: "Club",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Club",
                table: "Club",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Club_ClubId",
                table: "Players",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
