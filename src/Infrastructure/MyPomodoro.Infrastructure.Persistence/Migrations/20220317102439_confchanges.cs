using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPomodoro.Infrastructure.Persistence.Migrations
{
    public partial class confchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongBreak",
                table: "UserConfigurations");

            migrationBuilder.DropColumn(
                name: "LongBreakInterval",
                table: "UserConfigurations");

            migrationBuilder.DropColumn(
                name: "Pomodoro",
                table: "UserConfigurations");

            migrationBuilder.DropColumn(
                name: "PomodoroPeriodCount",
                table: "UserConfigurations");

            migrationBuilder.DropColumn(
                name: "ShortBreak",
                table: "UserConfigurations");

            migrationBuilder.AddColumn<bool>(
                name: "EmailNotification",
                table: "UserConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PushNotification",
                table: "UserConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LongBreakInterval",
                table: "Pomodoros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailNotification",
                table: "UserConfigurations");

            migrationBuilder.DropColumn(
                name: "PushNotification",
                table: "UserConfigurations");

            migrationBuilder.DropColumn(
                name: "LongBreakInterval",
                table: "Pomodoros");

            migrationBuilder.AddColumn<int>(
                name: "LongBreak",
                table: "UserConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LongBreakInterval",
                table: "UserConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pomodoro",
                table: "UserConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PomodoroPeriodCount",
                table: "UserConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShortBreak",
                table: "UserConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
