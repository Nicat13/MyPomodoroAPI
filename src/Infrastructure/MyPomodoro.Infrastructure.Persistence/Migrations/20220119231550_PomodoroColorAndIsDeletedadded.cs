using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPomodoro.Infrastructure.Persistence.Migrations
{
    public partial class PomodoroColorAndIsDeletedadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Pomodoros_PomodoroId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "PomodoroId",
                table: "Tasks",
                newName: "PomodoroSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_PomodoroId",
                table: "Tasks",
                newName: "IX_Tasks_PomodoroSessionId");

            migrationBuilder.AlterColumn<double>(
                name: "CurrentTime",
                table: "PomodoroSessions",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "Pomodoros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Pomodoros",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_PomodoroSessions_PomodoroSessionId",
                table: "Tasks",
                column: "PomodoroSessionId",
                principalTable: "PomodoroSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_PomodoroSessions_PomodoroSessionId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Pomodoros");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Pomodoros");

            migrationBuilder.RenameColumn(
                name: "PomodoroSessionId",
                table: "Tasks",
                newName: "PomodoroId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_PomodoroSessionId",
                table: "Tasks",
                newName: "IX_Tasks_PomodoroId");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentTime",
                table: "PomodoroSessions",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Pomodoros_PomodoroId",
                table: "Tasks",
                column: "PomodoroId",
                principalTable: "Pomodoros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
