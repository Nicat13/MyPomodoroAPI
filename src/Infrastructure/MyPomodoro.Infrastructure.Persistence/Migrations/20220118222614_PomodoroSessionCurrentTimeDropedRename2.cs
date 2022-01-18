using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPomodoro.Infrastructure.Persistence.Migrations
{
    public partial class PomodoroSessionCurrentTimeDropedRename2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTime",
                table: "PomodoroSessions");

            migrationBuilder.RenameColumn(
                name: "CurrentTime2",
                table: "PomodoroSessions",
                newName: "CurrentTime3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentTime3",
                table: "PomodoroSessions",
                newName: "CurrentTime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentTime",
                table: "PomodoroSessions",
                type: "datetime2",
                nullable: true);
        }
    }
}
