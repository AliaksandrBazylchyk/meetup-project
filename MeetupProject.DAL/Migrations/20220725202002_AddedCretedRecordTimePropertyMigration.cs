using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetupProject.DAL.Migrations
{
    public partial class AddedCretedRecordTimePropertyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RecordCreatedTime",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordCreatedTime",
                table: "Events");
        }
    }
}
