using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BestStudentCafedra.Models;
using System.Configuration;

#nullable disable

namespace BestStudentCafedra.Data
{
    public partial class SubjectAreaDbContext : DbContext
    {
        public SubjectAreaDbContext()
        {
        }

        public SubjectAreaDbContext(DbContextOptions<SubjectAreaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AcademicGroup> AcademicGroups { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ActivityProtection> ActivityProtections { get; set; }
        public virtual DbSet<ActivityType> ActivityTypes { get; set; }
        public virtual DbSet<AssignedStaff> AssignedStaffs { get; set; }
        public virtual DbSet<Discipline> Disciplines { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventTemplate> EventTemplates { get; set; }
        public virtual DbSet<EventLog> EventLogs { get; set; }
        public virtual DbSet<GraduationWork> GraduationWorks { get; set; }
        public virtual DbSet<GroupDiscipline> GroupDisciplines { get; set; }
        public virtual DbSet<ProposedTopic> ProposedTopics { get; set; }
        public virtual DbSet<SemesterDiscipline> SemesterDiscipline { get; set; }
        public virtual DbSet<Specialty> Specialties { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TeacherDiscipline> TeacherDisciplines { get; set; }
        public virtual DbSet<SchedulePlan> SchedulePlans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicGroup>(entity =>
            {
                entity.ToTable("academic_group");

                entity.HasIndex(e => e.SpecialtyId, "specialty_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FormationYear).HasColumnName("formation_year");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(20)")
                    .HasColumnName("name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SpecialtyId)
                    .IsRequired()
                    .HasColumnType("char(8)")
                    .HasColumnName("specialty_id")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Specialty)
                    .WithMany(p => p.AcademicGroups)
                    .HasForeignKey(d => d.SpecialtyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("academic_group_ibfk_1");
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("activity");

                entity.HasIndex(e => e.SemesterDisciplineId, "semester_discipline_id");

                entity.HasIndex(e => e.TypeId, "type_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SemesterDisciplineId).HasColumnName("semester_discipline_id");

                entity.Property(e => e.MaxPoints).HasColumnName("max_points");

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasColumnName("title")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.SemesterDiscipline)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.SemesterDisciplineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("activity_ibfk_2");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("activity_ibfk_1");
            });

            modelBuilder.Entity<ActivityProtection>(entity =>
            {
                entity.ToTable("activity_protection");

                entity.HasIndex(e => e.ActivityId, "activity_id");

                entity.HasIndex(e => e.StudentId, "student_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActivityId).HasColumnName("activity_id");

                entity.Property(e => e.Points).HasColumnName("points");

                entity.Property(e => e.ProtectionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("protection_date");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ActivityProtections)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("activity_protection_ibfk_2");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ActivityProtections)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("activity_protection_ibfk_1");
            });

            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.ToTable("activity_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(30)")
                    .HasColumnName("name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<AssignedStaff>(entity =>
            {
                entity.ToTable("assigned_staff");

                entity.HasIndex(e => e.GraduationWorkId, "graduation_work_id");

                entity.HasIndex(e => e.TeacherId, "teacher_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GraduationWorkId).HasColumnName("graduation_work_id");

                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("enum('Scientific Adviser','Reviewer')")
                    .HasColumnName("type")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.GraduationWork)
                    .WithMany(p => p.AssignedStaffs)
                    .HasForeignKey(d => d.GraduationWorkId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("assigned_staff_ibfk_1");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.AssignedStaff)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("assigned_staff_ibfk_2");
            });

            modelBuilder.Entity<Discipline>(entity =>
            {
                entity.ToTable("discipline");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<SemesterDiscipline>(entity =>
            {
                entity.ToTable("semester_discipline");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DisciplineId).HasColumnName("discipline_id");

                entity.HasIndex(e => e.DisciplineId, "discipline_id");

                entity.Property(e => e.ControlType)
                    .IsRequired()
                    .HasColumnType("enum('Exam','DifferentialCredit','Credit')")
                    .HasColumnName("control_type")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Semester).HasColumnName("semester");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Discipline)
                    .WithMany(p => p.SemesterDisciplines)
                    .HasForeignKey(d => d.DisciplineId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("semester_discipline_ibfk_1");
            });

            modelBuilder.Entity<EventTemplate>(entity =>
            {
                entity.ToTable("event_template");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SequentialNumber).HasColumnName("sequential_number");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(150)")
                    .HasColumnName("description")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.ToTable("event_log");

                entity.HasIndex(e => e.GraduationWorkId, "graduation_work_id");

                entity.HasIndex(e => e.EventId, "event_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GraduationWorkId).HasColumnName("graduation_work_id");

                entity.Property(e => e.Mark).HasColumnName("mark");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.HasOne(d => d.GraduationWork)
                    .WithMany(p => p.EventLogs)
                    .HasForeignKey(d => d.GraduationWorkId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("event_log_ibfk_1");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventLogs)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("event_log_ibfk_2");
            });

            modelBuilder.Entity<GraduationWork>(entity =>
            {
                entity.ToTable("graduation_work");

                entity.HasIndex(e => e.StudentId, "student_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArchievedDate)
                    .HasColumnType("date")
                    .HasColumnName("archieved_date");

                entity.Property(e => e.Result).HasColumnName("result");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Theme)
                    .HasColumnType("varchar(50)")
                    .HasColumnName("theme")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.GraduationWorks)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("graduation_works_ibfk_1");
            });

            modelBuilder.Entity<ProposedTopic>(entity =>
            {
                entity.ToTable("proposed_topic");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<GroupDiscipline>(entity =>
            {
                entity.ToTable("group_disciplines");

                entity.HasIndex(e => e.DisciplineId, "discipline_id");

                entity.HasIndex(e => e.GroupId, "group_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DisciplineId).HasColumnName("discipline_id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.HasOne(d => d.Discipline)
                    .WithMany(p => p.GroupDiscipline)
                    .HasForeignKey(d => d.DisciplineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_discipline_ibfk_1");

                entity.HasOne(d => d.AcademicGroup)
                    .WithMany(p => p.GroupDiscipline)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_discipline_ibfk_2");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event");

                entity.HasIndex(e => e.SchedulePlanId, "schedule_plan_id");

                entity.HasIndex(e => e.ResponsibleTeacherId, "responsible_teacher_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SchedulePlanId).HasColumnName("schedule_plan_id");

                entity.Property(e => e.ResponsibleTeacherId).HasColumnName("responsible_teacher_id");

                entity.Property(e => e.EventDescription)
                    .HasColumnType("varchar(150)")
                    .HasColumnName("event_description")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Class)
                    .HasColumnType("varchar(7)")
                    .HasColumnName("class")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.HasOne(d => d.SchedulePlan)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.SchedulePlanId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("schedule_plan_event_ibfk_1");

                entity.HasOne(d => d.ResponsibleTeacher)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.ResponsibleTeacherId)
                    .HasConstraintName("schedule_plan_event_ibfk_2");
            });

            modelBuilder.Entity<SchedulePlan>(entity =>
            {
                entity.ToTable("schedule_plan");

                entity.HasIndex(e => e.GroupId, "group_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.ApprovingOfficerName)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("approving_officer_name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ApprovedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("approved_date");

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_changed_date");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.SchedulePlans)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("schedule_plan_ibfk_1");
            });

            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PRIMARY");

                entity.ToTable("specialty");

                entity.HasIndex(e => e.Code, "code")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasColumnType("char(8)")
                    .HasColumnName("code")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.AcademicDegree)
                    .IsRequired()
                    .HasColumnType("enum('Undergraduate','Specialty','Magistracy','Postgraduate')")
                    .HasColumnName("academic_degree")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(50)")
                    .HasColumnName("name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.GradebookNumber)
                    .HasName("PRIMARY");

                entity.ToTable("student");

                entity.HasIndex(e => e.GroupId, "group_id");

                entity.Property(e => e.GradebookNumber)
                    .ValueGeneratedNever()
                    .HasColumnName("gradebook_number");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("full_name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnType("char(15)")
                    .HasColumnName("phone_number")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("student_ibfk_1");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("teacher");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AcademicDegree)
                    .HasColumnType("varchar(50)")
                    .HasColumnName("academic_degree")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("full_name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Post)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("post")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<TeacherDiscipline>(entity =>
            {
                entity.ToTable("teacher_disciplines");

                entity.HasIndex(e => e.DisciplineId, "discipline_id");

                entity.HasIndex(e => e.TeacherId, "teacher_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DisciplineId).HasColumnName("discipline_id");

                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.HasOne(d => d.Discipline)
                    .WithMany(p => p.TeacherDisciplines)
                    .HasForeignKey(d => d.DisciplineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("teacher_discipline_ibfk_1");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TeacherDisciplines)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("teacher_discipline_ibfk_2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
