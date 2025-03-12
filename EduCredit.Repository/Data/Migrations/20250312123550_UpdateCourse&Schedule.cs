using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCredit.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamDate",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Schedules",
                newName: "LectureStart");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExamDate",
                table: "Schedules",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ExamEnd",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "ExamLocation",
                table: "Schedules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ExamStart",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "LectureEnd",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "LectureLocation",
                table: "Schedules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamDate",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ExamEnd",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ExamLocation",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ExamStart",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "LectureEnd",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "LectureLocation",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "LectureStart",
                table: "Schedules",
                newName: "Time");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExamDate",
                table: "Courses",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
