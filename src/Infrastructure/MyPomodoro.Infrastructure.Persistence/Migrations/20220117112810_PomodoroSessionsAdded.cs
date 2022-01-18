using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPomodoro.Infrastructure.Persistence.Migrations
{
    public partial class PomodoroSessionsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Pomodoros_PomodoroId1",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PomodoroId1",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PomodoroId1",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CurrentStatus",
                table: "Pomodoros");

            migrationBuilder.DropColumn(
                name: "CurrentStep",
                table: "Pomodoros");

            migrationBuilder.DropColumn(
                name: "CurrentTime",
                table: "Pomodoros");

            migrationBuilder.DropColumn(
                name: "StatusChangeTime",
                table: "Pomodoros");

            migrationBuilder.AddColumn<int>(
                name: "PomodoroPeriodCount",
                table: "UserConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PomodoroId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PomodoroSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalPomodoroTime = table.Column<double>(type: "float", nullable: false),
                    TotalShortBreakTime = table.Column<double>(type: "float", nullable: false),
                    TotalLongBreakTime = table.Column<double>(type: "float", nullable: false),
                    CurrentPomodoroPeriod = table.Column<int>(type: "int", nullable: false),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    CurrentStatus = table.Column<int>(type: "int", nullable: false),
                    CurrentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusChangeTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PomodoroId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PomodoroSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PomodoroSession_Pomodoros_PomodoroId",
                        column: x => x.PomodoroId,
                        principalTable: "Pomodoros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PomodoroSession_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PomodoroId",
                table: "Tasks",
                column: "PomodoroId");

            migrationBuilder.CreateIndex(
                name: "IX_PomodoroSession_PomodoroId",
                table: "PomodoroSession",
                column: "PomodoroId");

            migrationBuilder.CreateIndex(
                name: "IX_PomodoroSession_UserId",
                table: "PomodoroSession",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Pomodoros_PomodoroId",
                table: "Tasks",
                column: "PomodoroId",
                principalTable: "Pomodoros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Pomodoros_PomodoroId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "PomodoroSession");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PomodoroId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PomodoroPeriodCount",
                table: "UserConfigurations");

            migrationBuilder.AlterColumn<string>(
                name: "PomodoroId",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PomodoroId1",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStatus",
                table: "Pomodoros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStep",
                table: "Pomodoros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentTime",
                table: "Pomodoros",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangeTime",
                table: "Pomodoros",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PomodoroId1",
                table: "Tasks",
                column: "PomodoroId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Pomodoros_PomodoroId1",
                table: "Tasks",
                column: "PomodoroId1",
                principalTable: "Pomodoros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
