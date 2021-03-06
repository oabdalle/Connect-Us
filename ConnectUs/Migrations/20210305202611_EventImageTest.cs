using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectUs.Migrations
{
    public partial class EventImageTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventImage_Event_EventId",
                table: "EventImage");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "EventImage");

            migrationBuilder.DropColumn(
                name: "HostID",
                table: "Event");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventImage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "EventImage",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EventImage_Event_EventId",
                table: "EventImage",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventImage_Event_EventId",
                table: "EventImage");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "EventImage");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventImage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "EventImage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HostID",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_EventImage_Event_EventId",
                table: "EventImage",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
