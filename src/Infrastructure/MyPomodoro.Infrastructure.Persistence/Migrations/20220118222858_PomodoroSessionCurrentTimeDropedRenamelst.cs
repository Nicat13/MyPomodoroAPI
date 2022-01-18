using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPomodoro.Infrastructure.Persistence.Migrations
{
    public partial class PomodoroSessionCurrentTimeDropedRenamelst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentTime3",
                table: "PomodoroSessions",
                newName: "CurrentTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentTime",
                table: "PomodoroSessions",
                newName: "CurrentTime3");
        }
    }
}
