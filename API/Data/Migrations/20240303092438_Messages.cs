using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class Messages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatchTaskId = table.Column<int>(type: "int", nullable: true),
                    BatchId = table.Column<int>(type: "int", nullable: true),
                    DestinationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRead = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_DestinationUserId",
                        column: x => x.DestinationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_BatchTasks_BatchTaskId",
                        column: x => x.BatchTaskId,
                        principalTable: "BatchTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_BatchTaskId",
                table: "Messages",
                column: "BatchTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DestinationUserId",
                table: "Messages",
                column: "DestinationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
