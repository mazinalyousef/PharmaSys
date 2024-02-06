using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class Tasktypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypeGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTypeGroups_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypeCheckLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskTypeId = table.Column<int>(type: "int", nullable: false),
                    TaskTypeGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypeCheckLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTypeCheckLists_TaskTypeGroups_TaskTypeGroupId",
                        column: x => x.TaskTypeGroupId,
                        principalTable: "TaskTypeGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskTypeCheckLists_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypeRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RangeValue = table.Column<decimal>(type: "decimal(32,2)", nullable: false),
                    TaskTypeId = table.Column<int>(type: "int", nullable: false),
                    TaskTypeGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypeRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTypeRanges_TaskTypeGroups_TaskTypeGroupId",
                        column: x => x.TaskTypeGroupId,
                        principalTable: "TaskTypeGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskTypeRanges_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeCheckLists_TaskTypeGroupId",
                table: "TaskTypeCheckLists",
                column: "TaskTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeCheckLists_TaskTypeId",
                table: "TaskTypeCheckLists",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeGroups_TaskTypeId",
                table: "TaskTypeGroups",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeRanges_TaskTypeGroupId",
                table: "TaskTypeRanges",
                column: "TaskTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeRanges_TaskTypeId",
                table: "TaskTypeRanges",
                column: "TaskTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskTypeCheckLists");

            migrationBuilder.DropTable(
                name: "TaskTypeRanges");

            migrationBuilder.DropTable(
                name: "TaskTypeGroups");

            migrationBuilder.DropTable(
                name: "TaskTypes");
        }
    }
}
