using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCredit.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTeacherScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Courses_CourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Schedules_ScheduleSemesterId_ScheduleCourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Semesters_SemesterId",
                table: "TeacherSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSchedules_CourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSchedules_ScheduleSemesterId_ScheduleCourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSchedules_SemesterId",
                table: "TeacherSchedules");

            migrationBuilder.DropColumn(
                name: "ScheduleCourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropColumn(
                name: "ScheduleSemesterId",
                table: "TeacherSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedules_CourseId_SemesterId",
                table: "TeacherSchedules",
                columns: new[] { "CourseId", "SemesterId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSchedules_Schedules_CourseId_SemesterId",
                table: "TeacherSchedules",
                columns: new[] { "CourseId", "SemesterId" },
                principalTable: "Schedules",
                principalColumns: new[] { "SemesterId", "CourseId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Schedules_CourseId_SemesterId",
                table: "TeacherSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSchedules_CourseId_SemesterId",
                table: "TeacherSchedules");

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleCourseId",
                table: "TeacherSchedules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleSemesterId",
                table: "TeacherSchedules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedules_CourseId",
                table: "TeacherSchedules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedules_ScheduleSemesterId_ScheduleCourseId",
                table: "TeacherSchedules",
                columns: new[] { "ScheduleSemesterId", "ScheduleCourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedules_SemesterId",
                table: "TeacherSchedules",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSchedules_Courses_CourseId",
                table: "TeacherSchedules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSchedules_Schedules_ScheduleSemesterId_ScheduleCourseId",
                table: "TeacherSchedules",
                columns: new[] { "ScheduleSemesterId", "ScheduleCourseId" },
                principalTable: "Schedules",
                principalColumns: new[] { "SemesterId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSchedules_Semesters_SemesterId",
                table: "TeacherSchedules",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
