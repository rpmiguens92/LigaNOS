using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LigaNOS.Migrations
{
    public partial class CreateEmployeesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
           name: "Employees",
           columns: table => new
           {
               Id = table.Column<int>(nullable: false)
                   .Annotation("SqlServer:Identity", "1, 1"),
               ImageFileId = table.Column<Guid>(nullable: false),
               Name = table.Column<string>(maxLength: 50, nullable: false),
               Address = table.Column<string>(nullable: true),
               Phone = table.Column<string>(nullable: true),
               Email = table.Column<string>(nullable: false),
               RoleId = table.Column<string>(nullable: false),
               ClubId = table.Column<int>(nullable: true)
           },
           constraints: table =>
           {
               table.PrimaryKey("PK_Employees", x => x.Id);
               table.ForeignKey(
                   name: "FK_Employees_AspNetRoles_RoleId",
                   column: x => x.RoleId,
                   principalTable: "AspNetRoles",
                   principalColumn: "Id",
                   onDelete: ReferentialAction.Cascade);
               table.ForeignKey(
                   name: "FK_Employees_Clubs_ClubId",
                   column: x => x.ClubId,
                   principalTable: "Clubs",
                   principalColumn: "Id",
                   onDelete: ReferentialAction.Restrict);
           });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ClubId",
                table: "Employees",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetRoles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Clubs_ClubId",
                table: "Employees",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetRoles_RoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Clubs_ClubId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ClubId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
