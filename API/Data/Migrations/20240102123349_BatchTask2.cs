using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class BatchTask2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchTaskCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchTaskId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchTaskCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchTaskCertificates_BatchTasks_BatchTaskId",
                        column: x => x.BatchTaskId,
                        principalTable: "BatchTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchTaskCheckedLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchTaskId = table.Column<int>(type: "int", nullable: false),
                    TaskTypeCheckListId = table.Column<int>(type: "int", nullable: true),
                    TaskTypeCheckListTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskTypeCheckListIsChecked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchTaskCheckedLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchTaskCheckedLists_BatchTasks_BatchTaskId",
                        column: x => x.BatchTaskId,
                        principalTable: "BatchTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchTaskNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchTaskId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchTaskNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchTaskNotes_BatchTasks_BatchTaskId",
                        column: x => x.BatchTaskId,
                        principalTable: "BatchTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchTaskRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchTaskId = table.Column<int>(type: "int", nullable: false),
                    TaskTypeRangeId = table.Column<int>(type: "int", nullable: true),
                    TaskTypeRangeTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskTypeRangeValue = table.Column<decimal>(type: "decimal(32,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchTaskRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchTaskRanges_BatchTasks_BatchTaskId",
                        column: x => x.BatchTaskId,
                        principalTable: "BatchTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchTaskCertificates_BatchTaskId",
                table: "BatchTaskCertificates",
                column: "BatchTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTaskCheckedLists_BatchTaskId",
                table: "BatchTaskCheckedLists",
                column: "BatchTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTaskNotes_BatchTaskId",
                table: "BatchTaskNotes",
                column: "BatchTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTaskRanges_BatchTaskId",
                table: "BatchTaskRanges",
                column: "BatchTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchTaskCertificates");

            migrationBuilder.DropTable(
                name: "BatchTaskCheckedLists");

            migrationBuilder.DropTable(
                name: "BatchTaskNotes");

            migrationBuilder.DropTable(
                name: "BatchTaskRanges");
        }
    }
}
