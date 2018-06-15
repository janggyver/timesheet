using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Timesheet.Migrations
{
    public partial class modifified2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Punchcard_AspNetUsers_UserId",
                table: "Punchcard");

            migrationBuilder.DropIndex(
                name: "IX_Punchcard_UserId",
                table: "Punchcard");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Punchcard",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Punchcard_ApplicationUserId",
                table: "Punchcard",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Punchcard_AspNetUsers_ApplicationUserId",
                table: "Punchcard",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Punchcard_AspNetUsers_ApplicationUserId",
                table: "Punchcard");

            migrationBuilder.DropIndex(
                name: "IX_Punchcard_ApplicationUserId",
                table: "Punchcard");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Punchcard");

            migrationBuilder.CreateIndex(
                name: "IX_Punchcard_UserId",
                table: "Punchcard",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Punchcard_AspNetUsers_UserId",
                table: "Punchcard",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
