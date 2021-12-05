using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPomodoro.Infrastructure.Persistence.Migrations
{
    public partial class userconfigurationsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POMODOROS_Users_UserId",
                table: "POMODOROS");

            migrationBuilder.DropForeignKey(
                name: "FK_TASKS_POMODOROS_PomodoroId1",
                table: "TASKS");

            migrationBuilder.DropForeignKey(
                name: "FK_TASKS_Users_UserId",
                table: "TASKS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TASKS",
                table: "TASKS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_POMODOROS",
                table: "POMODOROS");

            migrationBuilder.RenameTable(
                name: "TASKS",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "POMODOROS",
                newName: "Pomodoros");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Tasks",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_TASKS_UserId",
                table: "Tasks",
                newName: "IX_Tasks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TASKS_PomodoroId1",
                table: "Tasks",
                newName: "IX_Tasks_PomodoroId1");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Pomodoros",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_POMODOROS_UserId",
                table: "Pomodoros",
                newName: "IX_Pomodoros_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StatusChangeTime",
                table: "Pomodoros",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CurrentTime",
                table: "Pomodoros",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pomodoros",
                table: "Pomodoros",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutoStartPomodoros = table.Column<bool>(type: "bit", nullable: false),
                    AutoStartBreaks = table.Column<bool>(type: "bit", nullable: false),
                    LongBreakInterval = table.Column<int>(type: "int", nullable: false),
                    Pomodoro = table.Column<int>(type: "int", nullable: false),
                    ShortBreak = table.Column<int>(type: "int", nullable: false),
                    LongBreak = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserConfigurations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConfigurations_UserId",
                table: "UserConfigurations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pomodoros_Users_UserId",
                table: "Pomodoros",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Pomodoros_PomodoroId1",
                table: "Tasks",
                column: "PomodoroId1",
                principalTable: "Pomodoros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pomodoros_Users_UserId",
                table: "Pomodoros");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Pomodoros_PomodoroId1",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "UserConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pomodoros",
                table: "Pomodoros");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "TASKS");

            migrationBuilder.RenameTable(
                name: "Pomodoros",
                newName: "POMODOROS");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TASKS",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserId",
                table: "TASKS",
                newName: "IX_TASKS_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_PomodoroId1",
                table: "TASKS",
                newName: "IX_TASKS_PomodoroId1");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "POMODOROS",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Pomodoros_UserId",
                table: "POMODOROS",
                newName: "IX_POMODOROS_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StatusChangeTime",
                table: "POMODOROS",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CurrentTime",
                table: "POMODOROS",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TASKS",
                table: "TASKS",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_POMODOROS",
                table: "POMODOROS",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_POMODOROS_Users_UserId",
                table: "POMODOROS",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TASKS_POMODOROS_PomodoroId1",
                table: "TASKS",
                column: "PomodoroId1",
                principalTable: "POMODOROS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TASKS_Users_UserId",
                table: "TASKS",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
