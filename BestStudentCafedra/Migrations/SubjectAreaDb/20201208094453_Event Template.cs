using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class EventTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "event_log_ibfk_2",
                table: "event_log");

            migrationBuilder.DropTable(
                name: "schedule_plan_event");

            migrationBuilder.DropColumn(
                name: "description",
                table: "event");

            migrationBuilder.DropColumn(
                name: "name",
                table: "event");

            migrationBuilder.AlterColumn<string>(
                name: "control_type",
                table: "semester_discipline",
                type: "enum('Exam','DifferentialCredit','Credit')",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "enum('exam','differential credit','credit')",
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "approving_officer_name",
                table: "schedule_plan",
                type: "varchar(30)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(70)",
                oldNullable: true,
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "class",
                table: "event",
                type: "varchar(7)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "event",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "event_description",
                table: "event",
                type: "varchar(150)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "responsible_teacher_id",
                table: "event",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "schedule_plan_id",
                table: "event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "event_template",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sequential_number = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "varchar(150)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_template", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "responsible_teacher_id",
                table: "event",
                column: "responsible_teacher_id");

            migrationBuilder.CreateIndex(
                name: "schedule_plan_id",
                table: "event",
                column: "schedule_plan_id");

            migrationBuilder.AddForeignKey(
                name: "schedule_plan_event_ibfk_1",
                table: "event",
                column: "schedule_plan_id",
                principalTable: "schedule_plan",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "schedule_plan_event_ibfk_2",
                table: "event",
                column: "responsible_teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "event_log_ibfk_2",
                table: "event_log",
                column: "schedule_plan_event_id",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "schedule_plan_event_ibfk_1",
                table: "event");

            migrationBuilder.DropForeignKey(
                name: "schedule_plan_event_ibfk_2",
                table: "event");

            migrationBuilder.DropForeignKey(
                name: "event_log_ibfk_2",
                table: "event_log");

            migrationBuilder.DropTable(
                name: "event_template");

            migrationBuilder.DropIndex(
                name: "responsible_teacher_id",
                table: "event");

            migrationBuilder.DropIndex(
                name: "schedule_plan_id",
                table: "event");

            migrationBuilder.DropColumn(
                name: "class",
                table: "event");

            migrationBuilder.DropColumn(
                name: "date",
                table: "event");

            migrationBuilder.DropColumn(
                name: "event_description",
                table: "event");

            migrationBuilder.DropColumn(
                name: "responsible_teacher_id",
                table: "event");

            migrationBuilder.DropColumn(
                name: "schedule_plan_id",
                table: "event");

            migrationBuilder.AlterColumn<string>(
                name: "control_type",
                table: "semester_discipline",
                type: "enum('exam','differential credit','credit')",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "enum('Exam','DifferentialCredit','Credit')",
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "approving_officer_name",
                table: "schedule_plan",
                type: "varchar(70)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true,
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "event",
                type: "varchar(255)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "event",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "schedule_plan_event",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    @class = table.Column<string>(name: "class", type: "varchar(7)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date = table.Column<DateTime>(type: "datetime", nullable: true),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    responsible_teacher_id = table.Column<int>(type: "int", nullable: true),
                    schedule_plan_id = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "event_log_ibfk_2",
                table: "event_log",
                column: "schedule_plan_event_id",
                principalTable: "schedule_plan_event",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
