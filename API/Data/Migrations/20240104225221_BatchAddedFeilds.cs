using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class BatchAddedFeilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarttonPictureURL",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TubePictureURL",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TubeWeight",
                table: "Batches",
                type: "decimal(32,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarttonPictureURL",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "TubePictureURL",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "TubeWeight",
                table: "Batches");
        }
    }
}
