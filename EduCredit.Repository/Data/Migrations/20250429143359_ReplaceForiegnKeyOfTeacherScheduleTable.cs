using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCredit.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceForiegnKeyOfTeacherScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Schedules_CourseId_SemesterId",
                table: "TeacherSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSchedules_CourseId_SemesterId",
                table: "TeacherSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedules_SemesterId_CourseId",
                table: "TeacherSchedules",
                columns: new[] { "SemesterId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSchedules_Schedules_SemesterId_CourseId",
                table: "TeacherSchedules",
                columns: new[] { "SemesterId", "CourseId" },
                principalTable: "Schedules",
                principalColumns: new[] { "SemesterId", "CourseId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchedules_Schedules_SemesterId_CourseId",
                table: "TeacherSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSchedules_SemesterId_CourseId",
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
    }
}
