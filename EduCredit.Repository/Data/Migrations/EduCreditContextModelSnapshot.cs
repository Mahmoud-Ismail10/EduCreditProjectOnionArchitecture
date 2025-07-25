﻿// <auto-generated />
using System;
using EduCredit.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EduCredit.Repository.Data.Migrations
{
    [DbContext(typeof(EduCreditContext))]
    partial class EduCreditContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EduCredit.Core.Chat.ChatMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("SendAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("SenderId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("EduCredit.Core.Chat.UserCourseGroup", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserCourseGroups");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("CreditHours")
                        .HasColumnType("float");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<double>("MinimumDegree")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("PreviousCourseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("PreviousCourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DepartmentHeadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentHeadId")
                        .IsUnique()
                        .HasFilter("[DepartmentHeadId] IS NOT NULL");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("EduCredit.Core.Models.EnrollmentTable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GuideNotes")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("SemesterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StudentNotes")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("SemesterId");

                    b.HasIndex("StudentId");

                    b.ToTable("EnrollmentTables");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NationalId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("EduCredit.Core.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Semester", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EnrollmentClose")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EnrollmentOpen")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCourseGroupsCleared")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsEnrollmentTablesCleared")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Semesters");
                });

            modelBuilder.Entity("EduCredit.Core.Relations.Enrollment", b =>
                {
                    b.Property<Guid>("EnrollmentTableId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Appreciation")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<double?>("Grade")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<bool?>("IsPassAtCourse")
                        .HasColumnType("bit");

                    b.Property<double?>("Percentage")
                        .HasColumnType("float");

                    b.HasKey("EnrollmentTableId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("EduCredit.Core.Relations.Schedule", b =>
                {
                    b.Property<Guid>("SemesterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Day")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ExamDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("ExamEnd")
                        .HasColumnType("time");

                    b.Property<string>("ExamLocation")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<TimeSpan?>("ExamStart")
                        .HasColumnType("time");

                    b.Property<TimeSpan?>("LectureEnd")
                        .HasColumnType("time");

                    b.Property<string>("LectureLocation")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<TimeSpan?>("LectureStart")
                        .HasColumnType("time");

                    b.HasKey("SemesterId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("EduCredit.Core.Relations.TeacherSchedule", b =>
                {
                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SemesterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TeacherId", "SemesterId", "CourseId");

                    b.HasIndex("SemesterId", "CourseId");

                    b.ToTable("TeacherSchedules");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("EduCredit.Core.Models.Admin", b =>
                {
                    b.HasBaseType("EduCredit.Core.Models.Person");

                    b.ToTable("Admins", (string)null);
                });

            modelBuilder.Entity("EduCredit.Core.Models.Student", b =>
                {
                    b.HasBaseType("EduCredit.Core.Models.Person");

                    b.Property<double>("CreditHours")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("GPA")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Students", (string)null);
                });

            modelBuilder.Entity("EduCredit.Core.Models.Teacher", b =>
                {
                    b.HasBaseType("EduCredit.Core.Models.Person");

                    b.Property<DateOnly>("AppointmentDate")
                        .HasColumnType("date");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Teachers", (string)null);
                });

            modelBuilder.Entity("EduCredit.Core.Chat.ChatMessage", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Course", "Course")
                        .WithMany("ChatMessages")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduCredit.Core.Models.Person", "Sender")
                        .WithMany("ChatMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("EduCredit.Core.Chat.UserCourseGroup", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduCredit.Core.Models.Person", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Course", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Department", "Department")
                        .WithMany("Courses")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EduCredit.Core.Models.Course", "PreviousCourse")
                        .WithMany("NextCourses")
                        .HasForeignKey("PreviousCourseId");

                    b.Navigation("Department");

                    b.Navigation("PreviousCourse");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Department", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Teacher", "DepartmentHead")
                        .WithOne("HeadofDepartment")
                        .HasForeignKey("EduCredit.Core.Models.Department", "DepartmentHeadId");

                    b.Navigation("DepartmentHead");
                });

            modelBuilder.Entity("EduCredit.Core.Models.EnrollmentTable", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Semester", "Semester")
                        .WithMany("EnrollmentTables")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EduCredit.Core.Models.Student", "Student")
                        .WithMany("EnrollmentTables")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Semester");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("EduCredit.Core.Models.RefreshToken", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Person", "Person")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("EduCredit.Core.Relations.Enrollment", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduCredit.Core.Models.EnrollmentTable", "EnrollmentTable")
                        .WithMany("Enrollments")
                        .HasForeignKey("EnrollmentTableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("EnrollmentTable");
                });

            modelBuilder.Entity("EduCredit.Core.Relations.Schedule", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Course", "Course")
                        .WithMany("Schedules")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EduCredit.Core.Models.Semester", "Semester")
                        .WithMany("Schedules")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Semester");
                });

            modelBuilder.Entity("EduCredit.Core.Relations.TeacherSchedule", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Teacher", "Teacher")
                        .WithMany("TeacherSchedules")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduCredit.Core.Relations.Schedule", "Schedule")
                        .WithMany("TeacherSchedules")
                        .HasForeignKey("SemesterId", "CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduCredit.Core.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduCredit.Core.Models.Admin", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Person", null)
                        .WithOne()
                        .HasForeignKey("EduCredit.Core.Models.Admin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduCredit.Core.Models.Student", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Department", "Department")
                        .WithMany("Students")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EduCredit.Core.Models.Person", null)
                        .WithOne()
                        .HasForeignKey("EduCredit.Core.Models.Student", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduCredit.Core.Models.Teacher", "Teacher")
                        .WithMany("Students")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Department");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Teacher", b =>
                {
                    b.HasOne("EduCredit.Core.Models.Department", "Department")
                        .WithMany("Teachers")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EduCredit.Core.Models.Person", null)
                        .WithOne()
                        .HasForeignKey("EduCredit.Core.Models.Teacher", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Course", b =>
                {
                    b.Navigation("ChatMessages");

                    b.Navigation("Enrollments");

                    b.Navigation("NextCourses");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Department", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Students");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("EduCredit.Core.Models.EnrollmentTable", b =>
                {
                    b.Navigation("Enrollments");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Person", b =>
                {
                    b.Navigation("ChatMessages");

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Semester", b =>
                {
                    b.Navigation("EnrollmentTables");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("EduCredit.Core.Relations.Schedule", b =>
                {
                    b.Navigation("TeacherSchedules");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Student", b =>
                {
                    b.Navigation("EnrollmentTables");
                });

            modelBuilder.Entity("EduCredit.Core.Models.Teacher", b =>
                {
                    b.Navigation("HeadofDepartment");

                    b.Navigation("Students");

                    b.Navigation("TeacherSchedules");
                });
#pragma warning restore 612, 618
        }
    }
}
