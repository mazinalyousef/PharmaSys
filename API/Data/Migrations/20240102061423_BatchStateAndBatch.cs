using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class BatchStateAndBatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchStateId",
                table: "Batches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Batches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Batches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BatchStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchStates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_BatchStateId",
                table: "Batches",
                column: "BatchStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_BatchStates_BatchStateId",
                table: "Batches",
                column: "BatchStateId",
                principalTable: "BatchStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_BatchStates_BatchStateId",
                table: "Batches");

            migrationBuilder.DropTable(
                name: "BatchStates");

            migrationBuilder.DropIndex(
                name: "IX_Batches_BatchStateId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "BatchStateId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Batches");
        }
    }
}
