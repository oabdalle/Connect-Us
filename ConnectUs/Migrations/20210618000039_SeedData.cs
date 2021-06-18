using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectUs.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] {"Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                  {
                    {"1", "Admin", "Admin", "1"},
                    {"2", "Attendee", "Attendee", "1"},
                    {"3", "Creator", "Creator", "1"}

                  }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
