using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCredit.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingEnrollmentTableModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Students_StudentId",
                table: "Enrollments");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Enrollments",
                newName: "EnrollmentTableId");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)1);

            migrationBuilder.CreateTable(
                name: "EnrollmentTables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Session = table.Column<DateOnly>(type: "date", nullable: false),
                    Semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrollmentTables_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentTables_StudentId",
                table: "EnrollmentTables",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_EnrollmentTables_EnrollmentTableId",
                table: "Enrollments",
                column: "EnrollmentTableId",
                principalTable: "EnrollmentTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_EnrollmentTables_EnrollmentTableId",
                table: "Enrollments");

            migrationBuilder.DropTable(
                name: "EnrollmentTables");

            migrationBuilder.RenameColumn(
                name: "EnrollmentTableId",
                table: "Enrollments",
                newName: "StudentId");

            migrationBuilder.AlterColumn<byte>(
                name: "Level",
                table: "Students",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Students_StudentId",
                table: "Enrollments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
