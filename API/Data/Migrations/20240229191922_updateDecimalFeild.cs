using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class updateDecimalFeild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RangeValue",
                table: "TaskTypeRanges",
                type: "decimal(32,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Percentage",
                table: "ProductIngredients",
                type: "decimal(32,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaskTypeRangeValue",
                table: "BatchTaskRanges",
                type: "decimal(32,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QTYPerTube",
                table: "BatchIngredients",
                type: "decimal(32,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QTYPerBatch",
                table: "BatchIngredients",
                type: "decimal(32,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BatchSize",
                table: "Batches",
                type: "decimal(32,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,2)");

            migrationBuilder.AlterColumn<string>(
                name: "barcode",
                table: "Barcodes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TubeWeight",
                table: "Barcodes",
                type: "decimal(32,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Barcodes_barcode",
                table: "Barcodes",
                column: "barcode",
                unique: true,
                filter: "[barcode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Barcodes_barcode",
                table: "Barcodes");

            migrationBuilder.AlterColumn<decimal>(
                name: "RangeValue",
                table: "TaskTypeRanges",
                type: "decimal(32,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Percentage",
                table: "ProductIngredients",
                type: "decimal(32,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaskTypeRangeValue",
                table: "BatchTaskRanges",
                type: "decimal(32,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QTYPerTube",
                table: "BatchIngredients",
                type: "decimal(32,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QTYPerBatch",
                table: "BatchIngredients",
                type: "decimal(32,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BatchSize",
                table: "Batches",
                type: "decimal(32,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,3)");

            migrationBuilder.AlterColumn<string>(
                name: "barcode",
                table: "Barcodes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TubeWeight",
                table: "Barcodes",
                type: "decimal(32,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,3)");
        }
    }
}
