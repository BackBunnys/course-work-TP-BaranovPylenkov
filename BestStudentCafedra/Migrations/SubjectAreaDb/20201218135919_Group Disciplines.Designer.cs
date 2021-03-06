﻿// <auto-generated />
using System;
using BestStudentCafedra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    [DbContext(typeof(SubjectAreaDbContext))]
    [Migration("20201218135919_Group Disciplines")]
    partial class GroupDisciplines
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("BestStudentCafedra.Models.AcademicGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("FormationYear")
                        .HasColumnType("int")
                        .HasColumnName("formation_year");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("name")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<string>("SpecialtyId")
                        .IsRequired()
                        .HasColumnType("char(8)")
                        .HasColumnName("specialty_id")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "SpecialtyId" }, "specialty_id");

                    b.ToTable("academic_group");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("MaxPoints")
                        .HasColumnType("int")
                        .HasColumnName("max_points");

                    b.Property<int>("Number")
                        .HasColumnType("int")
                        .HasColumnName("number");

                    b.Property<int>("SemesterDisciplineId")
                        .HasColumnType("int")
                        .HasColumnName("semester_discipline_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("title")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<int?>("TypeId")
                        .HasColumnType("int")
                        .HasColumnName("type_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "SemesterDisciplineId" }, "semester_discipline_id");

                    b.HasIndex(new[] { "TypeId" }, "type_id");

                    b.ToTable("activity");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.ActivityProtection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("ActivityId")
                        .HasColumnType("int")
                        .HasColumnName("activity_id");

                    b.Property<int>("Points")
                        .HasColumnType("int")
                        .HasColumnName("points");

                    b.Property<DateTime>("ProtectionDate")
                        .HasColumnType("date")
                        .HasColumnName("protection_date");

                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ActivityId" }, "activity_id");

                    b.HasIndex(new[] { "StudentId" }, "student_id");

                    b.ToTable("activity_protection");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.ActivityType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasColumnName("name")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("activity_type");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.AssignedStaff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("GraduationWorkId")
                        .HasColumnType("int")
                        .HasColumnName("graduation_work_id");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int")
                        .HasColumnName("teacher_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("enum('Scientific Adviser','Reviewer')")
                        .HasColumnName("type")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "GraduationWorkId" }, "graduation_work_id");

                    b.HasIndex(new[] { "TeacherId" }, "teacher_id");

                    b.ToTable("assigned_staff");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Discipline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("discipline");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Class")
                        .HasColumnType("varchar(7)")
                        .HasColumnName("class")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime")
                        .HasColumnName("date");

                    b.Property<string>("EventDescription")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("event_description")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<int?>("ResponsibleTeacherId")
                        .HasColumnType("int")
                        .HasColumnName("responsible_teacher_id");

                    b.Property<int>("SchedulePlanId")
                        .HasColumnType("int")
                        .HasColumnName("schedule_plan_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ResponsibleTeacherId" }, "responsible_teacher_id");

                    b.HasIndex(new[] { "SchedulePlanId" }, "schedule_plan_id");

                    b.ToTable("event");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.EventLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("EventId")
                        .HasColumnType("int")
                        .HasColumnName("event_id");

                    b.Property<int>("GraduationWorkId")
                        .HasColumnType("int")
                        .HasColumnName("graduation_work_id");

                    b.Property<string>("Mark")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("mark");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "EventId" }, "event_id");

                    b.HasIndex(new[] { "GraduationWorkId" }, "graduation_work_id")
                        .HasDatabaseName("graduation_work_id1");

                    b.ToTable("event_log");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.EventTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("description")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<int>("SequentialNumber")
                        .HasColumnType("int")
                        .HasColumnName("sequential_number");

                    b.HasKey("Id");

                    b.ToTable("event_template");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.GraduationWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime?>("ArchievedDate")
                        .HasColumnType("date")
                        .HasColumnName("archieved_date");

                    b.Property<bool?>("Result")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("result");

                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    b.Property<string>("Theme")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("theme")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "StudentId" }, "student_id")
                        .HasDatabaseName("student_id1");

                    b.ToTable("graduation_work");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.GroupDiscipline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("DisciplineId")
                        .HasColumnType("int")
                        .HasColumnName("discipline_id");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DisciplineId" }, "discipline_id");

                    b.HasIndex(new[] { "GroupId" }, "group_id");

                    b.ToTable("group_disciplines");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.ProposedTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("name")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("proposed_topic");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.SchedulePlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("approved_date");

                    b.Property<string>("ApprovingOfficerName")
                        .HasColumnType("varchar(30)")
                        .HasColumnName("approving_officer_name")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<DateTime?>("LastChangedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("last_changed_date");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "GroupId" }, "group_id")
                        .HasDatabaseName("group_id1");

                    b.ToTable("schedule_plan");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.SemesterDiscipline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("ControlType")
                        .IsRequired()
                        .HasColumnType("enum('Exam','DifferentialCredit','Credit')")
                        .HasColumnName("control_type")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<int>("DisciplineId")
                        .HasColumnType("int")
                        .HasColumnName("discipline_id");

                    b.Property<int>("Semester")
                        .HasColumnType("int")
                        .HasColumnName("semester");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasColumnName("year");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DisciplineId" }, "discipline_id")
                        .HasDatabaseName("discipline_id1");

                    b.ToTable("semester_discipline");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Specialty", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("char(8)")
                        .HasColumnName("code")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<string>("AcademicDegree")
                        .IsRequired()
                        .HasColumnType("enum('undergraduate','specialty','magistracy','postgraduate')")
                        .HasColumnName("academic_degree")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Code" }, "code")
                        .IsUnique();

                    b.ToTable("specialty");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Student", b =>
                {
                    b.Property<int>("GradebookNumber")
                        .HasColumnType("int")
                        .HasColumnName("gradebook_number");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("full_name")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("char(15)")
                        .HasColumnName("phone_number")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.HasKey("GradebookNumber")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "GroupId" }, "group_id")
                        .HasDatabaseName("group_id2");

                    b.ToTable("student");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("AcademicDegree")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("academic_degree")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("full_name")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.Property<string>("Post")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("post")
                        .UseCollation("utf8mb4_0900_ai_ci")
                        .HasCharSet("utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("teacher");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.TeacherDiscipline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("DisciplineId")
                        .HasColumnType("int")
                        .HasColumnName("discipline_id");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DisciplineId" }, "discipline_id")
                        .HasDatabaseName("discipline_id2");

                    b.HasIndex(new[] { "TeacherId" }, "teacher_id")
                        .HasDatabaseName("teacher_id1");

                    b.ToTable("teacher_disciplines");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.AcademicGroup", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.Specialty", "Specialty")
                        .WithMany("AcademicGroups")
                        .HasForeignKey("SpecialtyId")
                        .HasConstraintName("academic_group_ibfk_1")
                        .IsRequired();

                    b.Navigation("Specialty");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Activity", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.SemesterDiscipline", "SemesterDiscipline")
                        .WithMany("Activities")
                        .HasForeignKey("SemesterDisciplineId")
                        .HasConstraintName("activity_ibfk_2")
                        .IsRequired();

                    b.HasOne("BestStudentCafedra.Models.ActivityType", "Type")
                        .WithMany("Activities")
                        .HasForeignKey("TypeId")
                        .HasConstraintName("activity_ibfk_1");

                    b.Navigation("SemesterDiscipline");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.ActivityProtection", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.Activity", "Activity")
                        .WithMany("ActivityProtections")
                        .HasForeignKey("ActivityId")
                        .HasConstraintName("activity_protection_ibfk_2")
                        .IsRequired();

                    b.HasOne("BestStudentCafedra.Models.Student", "Student")
                        .WithMany("ActivityProtections")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("activity_protection_ibfk_1")
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.AssignedStaff", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.GraduationWork", "GraduationWork")
                        .WithMany("AssignedStaffs")
                        .HasForeignKey("GraduationWorkId")
                        .HasConstraintName("assigned_staff_ibfk_1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BestStudentCafedra.Models.Teacher", "Teacher")
                        .WithMany("AssignedStaff")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("assigned_staff_ibfk_2")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GraduationWork");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Event", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.Teacher", "ResponsibleTeacher")
                        .WithMany("Events")
                        .HasForeignKey("ResponsibleTeacherId")
                        .HasConstraintName("schedule_plan_event_ibfk_2");

                    b.HasOne("BestStudentCafedra.Models.SchedulePlan", "SchedulePlan")
                        .WithMany("Events")
                        .HasForeignKey("SchedulePlanId")
                        .HasConstraintName("schedule_plan_event_ibfk_1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ResponsibleTeacher");

                    b.Navigation("SchedulePlan");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.EventLog", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.Event", "Event")
                        .WithMany("EventLogs")
                        .HasForeignKey("EventId")
                        .HasConstraintName("event_log_ibfk_2")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BestStudentCafedra.Models.GraduationWork", "GraduationWork")
                        .WithMany("EventLogs")
                        .HasForeignKey("GraduationWorkId")
                        .HasConstraintName("event_log_ibfk_1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("GraduationWork");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.GraduationWork", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.Student", "Student")
                        .WithMany("GraduationWorks")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("graduation_works_ibfk_1")
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.GroupDiscipline", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.Discipline", "Discipline")
                        .WithMany("GroupDiscipline")
                        .HasForeignKey("DisciplineId")
                        .HasConstraintName("group_discipline_ibfk_1")
                        .IsRequired();

                    b.HasOne("BestStudentCafedra.Models.AcademicGroup", "AcademicGroup")
                        .WithMany("GroupDiscipline")
                        .HasForeignKey("GroupId")
                        .HasConstraintName("group_discipline_ibfk_2")
                        .IsRequired();

                    b.Navigation("AcademicGroup");

                    b.Navigation("Discipline");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.SchedulePlan", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.AcademicGroup", "Group")
                        .WithMany("SchedulePlans")
                        .HasForeignKey("GroupId")
                        .HasConstraintName("schedule_plan_ibfk_1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.SemesterDiscipline", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.Discipline", "Discipline")
                        .WithMany("SemesterDisciplines")
                        .HasForeignKey("DisciplineId")
                        .HasConstraintName("semester_discipline_ibfk_1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discipline");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Student", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.AcademicGroup", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .HasConstraintName("student_ibfk_1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.TeacherDiscipline", b =>
                {
                    b.HasOne("BestStudentCafedra.Models.Discipline", "Discipline")
                        .WithMany("TeacherDisciplines")
                        .HasForeignKey("DisciplineId")
                        .HasConstraintName("teacher_discipline_ibfk_1")
                        .IsRequired();

                    b.HasOne("BestStudentCafedra.Models.Teacher", "Teacher")
                        .WithMany("TeacherDisciplines")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("teacher_discipline_ibfk_2")
                        .IsRequired();

                    b.Navigation("Discipline");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.AcademicGroup", b =>
                {
                    b.Navigation("GroupDiscipline");

                    b.Navigation("SchedulePlans");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Activity", b =>
                {
                    b.Navigation("ActivityProtections");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.ActivityType", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Discipline", b =>
                {
                    b.Navigation("GroupDiscipline");

                    b.Navigation("SemesterDisciplines");

                    b.Navigation("TeacherDisciplines");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Event", b =>
                {
                    b.Navigation("EventLogs");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.GraduationWork", b =>
                {
                    b.Navigation("AssignedStaffs");

                    b.Navigation("EventLogs");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.SchedulePlan", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.SemesterDiscipline", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Specialty", b =>
                {
                    b.Navigation("AcademicGroups");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Student", b =>
                {
                    b.Navigation("ActivityProtections");

                    b.Navigation("GraduationWorks");
                });

            modelBuilder.Entity("BestStudentCafedra.Models.Teacher", b =>
                {
                    b.Navigation("AssignedStaff");

                    b.Navigation("Events");

                    b.Navigation("TeacherDisciplines");
                });
#pragma warning restore 612, 618
        }
    }
}
