using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class Reviewersadvisers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "schedule_plan_event_ibfk_1",
                table: "event");

            migrationBuilder.DropForeignKey(
                name: "schedule_plan_event_ibfk_2",
                table: "event");

            migrationBuilder.DropTable(
                name: "assigned_staff");

            migrationBuilder.RenameIndex(
                name: "teacher_id1",
                table: "teacher_disciplines",
                newName: "teacher_id");

            migrationBuilder.RenameIndex(
                name: "graduation_work_id1",
                table: "event_log",
                newName: "graduation_work_id");

            migrationBuilder.AddColumn<int>(
                name: "reviewer_id",
                table: "graduation_work",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "scientific_adviser_id",
                table: "graduation_work",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "reviewer_id",
                table: "graduation_work",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "scientific_adviser_id",
                table: "graduation_work",
                column: "scientific_adviser_id");

            migrationBuilder.AddForeignKey(
                name: "event_ibfk_1",
                table: "event",
                column: "schedule_plan_id",
                principalTable: "schedule_plan",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "event_ibfk_2",
                table: "event",
                column: "responsible_teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_graduation_work_teacher_scientific_adviser_id",
                table: "graduation_work",
                column: "scientific_adviser_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "graduation_works_ibfk_3",
                table: "graduation_work",
                column: "student_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "event_ibfk_1",
                table: "event");

            migrationBuilder.DropForeignKey(
                name: "event_ibfk_2",
                table: "event");

            migrationBuilder.DropForeignKey(
                name: "FK_graduation_work_teacher_scientific_adviser_id",
                table: "graduation_work");

            migrationBuilder.DropForeignKey(
                name: "graduation_works_ibfk_3",
                table: "graduation_work");

            migrationBuilder.DropIndex(
                name: "reviewer_id",
                table: "graduation_work");

            migrationBuilder.DropIndex(
                name: "scientific_adviser_id",
                table: "graduation_work");

            migrationBuilder.DropColumn(
                name: "reviewer_id",
                table: "graduation_work");

            migrationBuilder.DropColumn(
                name: "scientific_adviser_id",
                table: "graduation_work");

            migrationBuilder.RenameIndex(
                name: "teacher_id",
                table: "teacher_disciplines",
                newName: "teacher_id1");

            migrationBuilder.RenameIndex(
                name: "graduation_work_id",
                table: "event_log",
                newName: "graduation_work_id1");

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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "assigned_staff_ibfk_2",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "graduation_work_id",
                table: "assigned_staff",
                column: "graduation_work_id");

            migrationBuilder.CreateIndex(
                name: "teacher_id",
                table: "assigned_staff",
                column: "teacher_id");

            migrationBuilder.AddForeignKey(
                name: "schedule_plan_event_ibfk_1",
                table: "event",
                column: "schedule_plan_id",
                principalTable: "schedule_plan",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "schedule_plan_event_ibfk_2",
                table: "event",
                column: "responsible_teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
