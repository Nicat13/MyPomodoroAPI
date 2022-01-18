using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPomodoro.Infrastructure.Persistence.Migrations
{
    public partial class PomodoroSessionCurrentTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentTime2",
                table: "PomodoroSessions",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTime2",
                table: "PomodoroSessions");
        }
    }
}
