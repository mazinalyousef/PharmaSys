using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class BatchTaskIngredientCheckList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchTaskIngredientCheckLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchTaskId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: true),
                    QTYPerTube = table.Column<decimal>(type: "decimal(32,2)", nullable: false),
                    QTYPerBatch = table.Column<decimal>(type: "decimal(32,2)", nullable: false),
                    QTYPerBatchIsChecked = table.Column<bool>(type: "bit", nullable: false)
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
                name: "IX_BatchTaskIngredientCheckLists_BatchTaskId",
                table: "BatchTaskIngredientCheckLists",
                column: "BatchTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchTaskIngredientCheckLists");
        }
    }
}
