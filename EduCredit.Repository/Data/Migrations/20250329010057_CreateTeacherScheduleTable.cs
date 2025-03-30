using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCredit.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTeacherScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Courses_CourseId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_CourseId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Schedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "CourseId");

            migrationBuilder.CreateTable(
                name: "TeacherSchedules",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSchedules", x => new { x.TeacherId, x.ScheduleId });
                    table.ForeignKey(
                        name: "FK_TeacherSchedules_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherSchedules_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedules_ScheduleId",
                table: "TeacherSchedules",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Courses_CourseId",
                table: "Schedules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Courses_CourseId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "TeacherSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Schedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                columns: new[] { "TeacherId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_CourseId",
                table: "Schedules",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Courses_CourseId",
                table: "Schedules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                table: "Schedules",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
