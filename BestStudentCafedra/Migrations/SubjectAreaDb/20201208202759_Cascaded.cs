using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class Cascaded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "assigned_staff_ibfk_1",
                table: "assigned_staff");

            migrationBuilder.DropForeignKey(
                name: "assigned_staff_ibfk_2",
                table: "assigned_staff");

            migrationBuilder.DropForeignKey(
                name: "schedule_plan_event_ibfk_1",
                table: "event");

            migrationBuilder.DropForeignKey(
                name: "event_log_ibfk_1",
                table: "event_log");

            migrationBuilder.DropForeignKey(
                name: "event_log_ibfk_2",
                table: "event_log");

            migrationBuilder.DropForeignKey(
                name: "schedule_plan_ibfk_1",
                table: "schedule_plan");

            migrationBuilder.DropForeignKey(
                name: "student_ibfk_1",
                table: "student");

            migrationBuilder.AlterColumn<string>(
                name: "event_description",
                table: "event",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true,
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "assigned_staff_ibfk_1",
                table: "assigned_staff",
                column: "graduation_work_id",
                principalTable: "graduation_work",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "assigned_staff_ibfk_2",
                table: "assigned_staff",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "schedule_plan_event_ibfk_1",
                table: "event",
                column: "schedule_plan_id",
                principalTable: "schedule_plan",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "event_log_ibfk_1",
                table: "event_log",
                column: "graduation_work_id",
                principalTable: "graduation_work",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "event_log_ibfk_2",
                table: "event_log",
                column: "event_id",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "schedule_plan_ibfk_1",
                table: "schedule_plan",
                column: "group_id",
                principalTable: "academic_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "student_ibfk_1",
                table: "student",
                column: "group_id",
                principalTable: "academic_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "assigned_staff_ibfk_1",
                table: "assigned_staff");

            migrationBuilder.DropForeignKey(
                name: "assigned_staff_ibfk_2",
                table: "assigned_staff");

            migrationBuilder.DropForeignKey(
                name: "schedule_plan_event_ibfk_1",
                table: "event");

            migrationBuilder.DropForeignKey(
                name: "event_log_ibfk_1",
                table: "event_log");

            migrationBuilder.DropForeignKey(
                name: "event_log_ibfk_2",
                table: "event_log");

            migrationBuilder.DropForeignKey(
                name: "schedule_plan_ibfk_1",
                table: "schedule_plan");

            migrationBuilder.DropForeignKey(
                name: "student_ibfk_1",
                table: "student");

            migrationBuilder.AlterColumn<string>(
                name: "event_description",
                table: "event",
                type: "varchar(150)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150,
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "assigned_staff_ibfk_1",
                table: "assigned_staff",
                column: "graduation_work_id",
                principalTable: "graduation_work",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "assigned_staff_ibfk_2",
                table: "assigned_staff",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "schedule_plan_event_ibfk_1",
                table: "event",
                column: "schedule_plan_id",
                principalTable: "schedule_plan",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "event_log_ibfk_1",
                table: "event_log",
                column: "graduation_work_id",
                principalTable: "graduation_work",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "event_log_ibfk_2",
                table: "event_log",
                column: "event_id",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "schedule_plan_ibfk_1",
                table: "schedule_plan",
                column: "group_id",
                principalTable: "academic_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "student_ibfk_1",
                table: "student",
                column: "group_id",
                principalTable: "academic_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
