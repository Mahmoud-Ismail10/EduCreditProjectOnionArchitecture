using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCredit.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Schedules_ScheduleId",
                table: "TeacherSchedules");

            migrationBuilder.DropTable(
                name: "SemesterCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherSchedules",
                table: "TeacherSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "TeacherSchedules",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSchedules_ScheduleId",
                table: "TeacherSchedules",
                newName: "IX_TeacherSchedules_CourseId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Schedules",
                newName: "SemesterId");

            migrationBuilder.AddColumn<Guid>(
                name: "SemesterId",
                table: "TeacherSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherSchedules",
                table: "TeacherSchedules",
                columns: new[] { "TeacherId", "SemesterId", "CourseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                columns: new[] { "SemesterId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedules_ScheduleSemesterId_ScheduleCourseId",
                table: "TeacherSchedules",
                columns: new[] { "ScheduleSemesterId", "ScheduleCourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedules_SemesterId",
                table: "TeacherSchedules",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Semesters_SemesterId",
                table: "Schedules",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Semesters_SemesterId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Courses_CourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Schedules_ScheduleSemesterId_ScheduleCourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Semesters_SemesterId",
                table: "TeacherSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherSchedules",
                table: "TeacherSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSchedules_ScheduleSemesterId_ScheduleCourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSchedules_SemesterId",
                table: "TeacherSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "TeacherSchedules");

            migrationBuilder.DropColumn(
                name: "ScheduleCourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropColumn(
                name: "ScheduleSemesterId",
                table: "TeacherSchedules");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "TeacherSchedules",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSchedules_CourseId",
                table: "TeacherSchedules",
                newName: "IX_TeacherSchedules_ScheduleId");

            migrationBuilder.RenameColumn(
                name: "SemesterId",
                table: "Schedules",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherSchedules",
                table: "TeacherSchedules",
                columns: new[] { "TeacherId", "ScheduleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SemesterCourses",
                columns: table => new
                {
                    SemesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterCourses", x => new { x.SemesterId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_SemesterCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemesterCourses_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SemesterCourses_CourseId",
                table: "SemesterCourses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSchedules_Schedules_ScheduleId",
                table: "TeacherSchedules",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
