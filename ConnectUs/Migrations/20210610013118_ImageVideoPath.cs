using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectUs.Migrations
{
    public partial class ImageVideoPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoBase64",
                table: "Product",
                newName: "VideoPath");

            migrationBuilder.RenameColumn(
                name: "ImageBase64",
                table: "Product",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "EventImage",
                newName: "ImagePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoPath",
                table: "Product",
                newName: "VideoBase64");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Product",
                newName: "ImageBase64");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "EventImage",
                newName: "Content");
        }
    }
}
