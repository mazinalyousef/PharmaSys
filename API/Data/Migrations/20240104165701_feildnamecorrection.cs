using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class feildnamecorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarttonPictureURL",
                table: "Batches",
                newName: "CartoonPictureURL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartoonPictureURL",
                table: "Batches",
                newName: "CarttonPictureURL");
        }
    }
}
