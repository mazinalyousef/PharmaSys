using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class dropunnecessaryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchTaskCheckedLists");

            migrationBuilder.DropTable(
                name: "BatchTaskIngredientCheckLists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchTaskCheckedLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchTaskId = table.Column<int>(type: "int", nullable: false),
                    TaskTypeCheckListId = table.Column<int>(type: "int", nullable: true),
                    TaskTypeCheckListIsChecked = table.Column<bool>(type: "bit", nullable: false),
                    TaskTypeCheckListTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "BatchTaskIngredientCheckLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchTaskId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: true),
                    QTYPerBatch = table.Column<decimal>(type: "decimal(32,2)", nullable: false),
                    QTYPerBatchIsChecked = table.Column<bool>(type: "bit", nullable: false),
                    QTYPerTube = table.Column<decimal>(type: "decimal(32,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchTaskIngredientCheckLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchTaskIngredientCheckLists_BatchTasks_BatchTaskId",
                        column: x => x.BatchTaskId,
                        principalTable: "BatchTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchTaskCheckedLists_BatchTaskId",
                table: "BatchTaskCheckedLists",
                column: "BatchTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTaskIngredientCheckLists_BatchTaskId",
                table: "BatchTaskIngredientCheckLists",
                column: "BatchTaskId");
        }
    }
}
