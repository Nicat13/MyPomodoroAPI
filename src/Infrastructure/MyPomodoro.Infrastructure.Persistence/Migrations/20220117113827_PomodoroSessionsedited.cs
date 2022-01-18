using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPomodoro.Infrastructure.Persistence.Migrations
{
    public partial class PomodoroSessionsedited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PomodoroSession_Pomodoros_PomodoroId",
                table: "PomodoroSession");

            migrationBuilder.DropForeignKey(
                name: "FK_PomodoroSession_Users_UserId",
                table: "PomodoroSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PomodoroSession",
                table: "PomodoroSession");

            migrationBuilder.RenameTable(
                name: "PomodoroSession",
                newName: "PomodoroSessions");

            migrationBuilder.RenameIndex(
                name: "IX_PomodoroSession_UserId",
                table: "PomodoroSessions",
                newName: "IX_PomodoroSessions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PomodoroSession_PomodoroId",
                table: "PomodoroSessions",
                newName: "IX_PomodoroSessions_PomodoroId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PomodoroSessions",
                table: "PomodoroSessions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PomodoroSessions_Pomodoros_PomodoroId",
                table: "PomodoroSessions",
                column: "PomodoroId",
                principalTable: "Pomodoros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PomodoroSessions_Users_UserId",
                table: "PomodoroSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PomodoroSessions_Pomodoros_PomodoroId",
                table: "PomodoroSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_PomodoroSessions_Users_UserId",
                table: "PomodoroSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PomodoroSessions",
                table: "PomodoroSessions");

            migrationBuilder.RenameTable(
                name: "PomodoroSessions",
                newName: "PomodoroSession");

            migrationBuilder.RenameIndex(
                name: "IX_PomodoroSessions_UserId",
                table: "PomodoroSession",
                newName: "IX_PomodoroSession_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PomodoroSessions_PomodoroId",
                table: "PomodoroSession",
                newName: "IX_PomodoroSession_PomodoroId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PomodoroSession",
                table: "PomodoroSession",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PomodoroSession_Pomodoros_PomodoroId",
                table: "PomodoroSession",
                column: "PomodoroId",
                principalTable: "Pomodoros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PomodoroSession_Users_UserId",
                table: "PomodoroSession",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
