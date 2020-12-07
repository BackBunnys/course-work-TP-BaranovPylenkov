using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activity_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(30)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discipline",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    year = table.Column<int>(type: "int", nullable: false),
                    semester = table.Column<int>(type: "int", nullable: false),
                    control_type = table.Column<string>(type: "enum('exam','differential credit','credit')", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discipline", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "proposed_topic",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proposed_topic", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "specialty",
                columns: table => new
                {
                    code = table.Column<string>(type: "char(8)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    academic_degree = table.Column<string>(type: "enum('undergraduate','specialty','magistracy','postgraduate')", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "teacher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    full_name = table.Column<string>(type: "varchar(100)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    post = table.Column<string>(type: "varchar(50)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    academic_degree = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "activity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type_id = table.Column<int>(type: "int", nullable: true),
                    discipline_id = table.Column<int>(type: "int", nullable: false),
                    number = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    max_points = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity", x => x.id);
                    table.ForeignKey(
                        name: "activity_ibfk_1",
                        column: x => x.type_id,
                        principalTable: "activity_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "activity_ibfk_2",
                        column: x => x.discipline_id,
                        principalTable: "discipline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "academic_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    specialty_id = table.Column<string>(type: "char(8)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(20)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    formation_year = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_academic_group", x => x.id);
                    table.ForeignKey(
                        name: "academic_group_ibfk_1",
                        column: x => x.specialty_id,
                        principalTable: "specialty",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teacher_disciplines",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    teacher_id = table.Column<int>(type: "int", nullable: false),
                    discipline_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_disciplines", x => x.id);
                    table.ForeignKey(
                        name: "teacher_discipline_ibfk_1",
                        column: x => x.discipline_id,
                        principalTable: "discipline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "teacher_discipline_ibfk_2",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "schedule_plan",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    group_id = table.Column<int>(type: "int", nullable: false),
                    approved_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    last_changed_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    approving_officer_name = table.Column<string>(type: "varchar(70)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_plan", x => x.id);
                    table.ForeignKey(
                        name: "schedule_plan_ibfk_1",
                        column: x => x.group_id,
                        principalTable: "academic_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    gradebook_number = table.Column<int>(type: "int", nullable: false),
                    group_id = table.Column<int>(type: "int", nullable: false),
                    full_name = table.Column<string>(type: "varchar(100)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "char(15)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.gradebook_number);
                    table.ForeignKey(
                        name: "student_ibfk_1",
                        column: x => x.group_id,
                        principalTable: "academic_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "schedule_plan_event",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    schedule_plan_id = table.Column<int>(type: "int", nullable: false),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: true),
                    @class = table.Column<string>(name: "class", type: "varchar(7)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    responsible_teacher_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_plan_event", x => x.id);
                    table.ForeignKey(
                        name: "schedule_plan_event_ibfk_1",
                        column: x => x.schedule_plan_id,
                        principalTable: "schedule_plan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "schedule_plan_event_ibfk_2",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "schedule_plan_event_ibfk_3",
                        column: x => x.responsible_teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "activity_protection",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    activity_id = table.Column<int>(type: "int", nullable: false),
                    protection_date = table.Column<DateTime>(type: "date", nullable: false),
                    points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_protection", x => x.id);
                    table.ForeignKey(
                        name: "activity_protection_ibfk_1",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "gradebook_number",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "activity_protection_ibfk_2",
                        column: x => x.activity_id,
                        principalTable: "activity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "graduation_work",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    theme = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    archieved_date = table.Column<DateTime>(type: "date", nullable: true),
                    result = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_graduation_work", x => x.id);
                    table.ForeignKey(
                        name: "graduation_works_ibfk_1",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "gradebook_number",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assigned_staff",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    graduation_work_id = table.Column<int>(type: "int", nullable: false),
                    teacher_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "enum('Scientific Adviser','Reviewer')", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assigned_staff", x => x.id);
                    table.ForeignKey(
                        name: "assigned_staff_ibfk_1",
                        column: x => x.graduation_work_id,
                        principalTable: "graduation_work",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "assigned_staff_ibfk_2",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "event_log",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    graduation_work_id = table.Column<int>(type: "int", nullable: false),
                    schedule_plan_event_id = table.Column<int>(type: "int", nullable: false),
                    mark = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_log", x => x.id);
                    table.ForeignKey(
                        name: "event_log_ibfk_1",
                        column: x => x.graduation_work_id,
                        principalTable: "graduation_work",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "event_log_ibfk_2",
                        column: x => x.schedule_plan_event_id,
                        principalTable: "schedule_plan_event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "specialty_id",
                table: "academic_group",
                column: "specialty_id");

            migrationBuilder.CreateIndex(
                name: "discipline_id",
                table: "activity",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "type_id",
                table: "activity",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "activity_id",
                table: "activity_protection",
                column: "activity_id");

            migrationBuilder.CreateIndex(
                name: "student_id",
                table: "activity_protection",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "graduation_work_id",
                table: "assigned_staff",
                column: "graduation_work_id");

            migrationBuilder.CreateIndex(
                name: "teacher_id",
                table: "assigned_staff",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "graduation_work_id1",
                table: "event_log",
                column: "graduation_work_id");

            migrationBuilder.CreateIndex(
                name: "schedule_plan_event_id",
                table: "event_log",
                column: "schedule_plan_event_id");

            migrationBuilder.CreateIndex(
                name: "student_id1",
                table: "graduation_work",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "group_id",
                table: "schedule_plan",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "event_id",
                table: "schedule_plan_event",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "responsible_teacher_id",
                table: "schedule_plan_event",
                column: "responsible_teacher_id");

            migrationBuilder.CreateIndex(
                name: "schedule_plan_id",
                table: "schedule_plan_event",
                column: "schedule_plan_id");

            migrationBuilder.CreateIndex(
                name: "code",
                table: "specialty",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "group_id1",
                table: "student",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "discipline_id1",
                table: "teacher_disciplines",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "teacher_id1",
                table: "teacher_disciplines",
                column: "teacher_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_protection");

            migrationBuilder.DropTable(
                name: "assigned_staff");

            migrationBuilder.DropTable(
                name: "event_log");

            migrationBuilder.DropTable(
                name: "proposed_topic");

            migrationBuilder.DropTable(
                name: "teacher_disciplines");

            migrationBuilder.DropTable(
                name: "activity");

            migrationBuilder.DropTable(
                name: "graduation_work");

            migrationBuilder.DropTable(
                name: "schedule_plan_event");

            migrationBuilder.DropTable(
                name: "activity_type");

            migrationBuilder.DropTable(
                name: "discipline");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "schedule_plan");

            migrationBuilder.DropTable(
                name: "event");

            migrationBuilder.DropTable(
                name: "teacher");

            migrationBuilder.DropTable(
                name: "academic_group");

            migrationBuilder.DropTable(
                name: "specialty");
        }
    }
}
