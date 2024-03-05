using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class addRequiredConstriants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductName",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_IngredientName",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Barcodes_barcode",
                table: "Barcodes");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IngredientName",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "barcode",
                table: "Barcodes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName",
                table: "Products",
                column: "ProductName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientName",
                table: "Ingredients",
                column: "IngredientName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Barcodes_barcode",
                table: "Barcodes",
                column: "barcode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductName",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_IngredientName",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Barcodes_barcode",
                table: "Barcodes");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IngredientName",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "barcode",
                table: "Barcodes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName",
                table: "Products",
                column: "ProductName",
                unique: true,
                filter: "[ProductName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientName",
                table: "Ingredients",
                column: "IngredientName",
                unique: true,
                filter: "[IngredientName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Barcodes_barcode",
                table: "Barcodes",
                column: "barcode",
                unique: true,
                filter: "[barcode] IS NOT NULL");
        }
    }
}
