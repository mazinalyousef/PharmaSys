using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class addBatchIngredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchIngredient_Batches_BatchId",
                table: "BatchIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_BatchIngredient_Ingredients_IngredientId",
                table: "BatchIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BatchIngredient",
                table: "BatchIngredient");

            migrationBuilder.RenameTable(
                name: "BatchIngredient",
                newName: "BatchIngredients");

            migrationBuilder.RenameIndex(
                name: "IX_BatchIngredient_IngredientId",
                table: "BatchIngredients",
                newName: "IX_BatchIngredients_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_BatchIngredient_BatchId",
                table: "BatchIngredients",
                newName: "IX_BatchIngredients_BatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BatchIngredients",
                table: "BatchIngredients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchIngredients_Batches_BatchId",
                table: "BatchIngredients",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BatchIngredients_Ingredients_IngredientId",
                table: "BatchIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchIngredients_Batches_BatchId",
                table: "BatchIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_BatchIngredients_Ingredients_IngredientId",
                table: "BatchIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BatchIngredients",
                table: "BatchIngredients");

            migrationBuilder.RenameTable(
                name: "BatchIngredients",
                newName: "BatchIngredient");

            migrationBuilder.RenameIndex(
                name: "IX_BatchIngredients_IngredientId",
                table: "BatchIngredient",
                newName: "IX_BatchIngredient_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_BatchIngredients_BatchId",
                table: "BatchIngredient",
                newName: "IX_BatchIngredient_BatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BatchIngredient",
                table: "BatchIngredient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchIngredient_Batches_BatchId",
                table: "BatchIngredient",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BatchIngredient_Ingredients_IngredientId",
                table: "BatchIngredient",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
