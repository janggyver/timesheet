using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Timesheet.Migrations
{
    public partial class addedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Punchcard",
                columns: table => new
                {
                    PunchcardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BreakTime = table.Column<decimal>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    WorkDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Punchcard", x => x.PunchcardId);
                    table.ForeignKey(
                        name: "FK_Punchcard_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Punchcard_UserId",
                table: "Punchcard",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Punchcard");
        }
    }
}
