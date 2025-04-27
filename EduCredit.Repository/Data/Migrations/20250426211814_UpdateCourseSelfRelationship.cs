using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCredit.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseSelfRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_PreviousCourseId",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PreviousCourseId",
                table: "Courses",
                column: "PreviousCourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_PreviousCourseId",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PreviousCourseId",
                table: "Courses",
                column: "PreviousCourseId",
                unique: true,
                filter: "[PreviousCourseId] IS NOT NULL");
        }
    }
}
