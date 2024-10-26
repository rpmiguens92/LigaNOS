using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaNOS.Migrations
{
    public partial class updatedDBUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageFileId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_userId",
                table: "AspNetUsers",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_userId",
                table: "AspNetUsers",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_userId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_userId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageFileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "AspNetUsers");
        }
    }
}
