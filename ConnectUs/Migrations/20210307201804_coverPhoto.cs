using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectUs.Migrations
{
    public partial class coverPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cover",
                table: "EventImage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Event_CategoryId",
                table: "Event",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Category_CategoryId",
                table: "Event",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Category_CategoryId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_CategoryId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Cover",
                table: "EventImage");
        }
    }
}
