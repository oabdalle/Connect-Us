using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectUs.Migrations
{
    public partial class MyVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoBase64",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoBase64",
                table: "Product");
        }
    }
}
