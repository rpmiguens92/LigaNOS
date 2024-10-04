using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaNOS.Migrations
{
    public partial class addUsersPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ImageFile",
                table: "Clubs",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageFile",
                table: "Clubs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
