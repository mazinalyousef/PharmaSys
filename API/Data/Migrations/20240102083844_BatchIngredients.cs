using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class BatchIngredients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchIngredient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    QTYPerTube = table.Column<decimal>(type: "decimal(32,2)", nullable: false),
                    QTYPerBatch = table.Column<decimal>(type: "decimal(32,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchIngredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchIngredient_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchIngredient_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchIngredient_BatchId",
                table: "BatchIngredient",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchIngredient_IngredientId",
                table: "BatchIngredient",
                column: "IngredientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchIngredient");
        }
    }
}
