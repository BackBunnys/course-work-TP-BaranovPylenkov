using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class Cascaded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "specialty",
                type: "varchar(50)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "academic_degree",
                table: "specialty",
                type: "enum('Undergraduate','Specialty','Magistracy','Postgraduate')",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "enum('undergraduate','specialty','magistracy','postgraduate')",
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "result",
                table: "graduation_work",
                type: "int",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "activity_ibfk_2",
                table: "activity",
                column: "semester_discipline_id",
                principalTable: "semester_discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "activity_protection_ibfk_1",
                table: "activity_protection",
                column: "student_id",
                principalTable: "student",
                principalColumn: "gradebook_number",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "activity_protection_ibfk_2",
                table: "activity_protection",
                column: "activity_id",
                principalTable: "activity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "group_discipline_ibfk_1",
                table: "group_disciplines",
                column: "discipline_id",
                principalTable: "discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "group_discipline_ibfk_2",
                table: "group_disciplines",
                column: "group_id",
                principalTable: "academic_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "rating_control_ibfk_2",
                table: "rating_control",
                column: "semester_discipline_id",
                principalTable: "semester_discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "rating_control_ibfk_3",
                table: "rating_control",
                column: "group_id",
                principalTable: "academic_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "student_rating_ibfk_1",
                table: "student_rating",
                column: "student_id",
                principalTable: "student",
                principalColumn: "gradebook_number",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "student_rating_ibfk_2",
                table: "student_rating",
                column: "rating_id",
                principalTable: "rating_control",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "teacher_discipline_ibfk_1",
                table: "teacher_disciplines",
                column: "discipline_id",
                principalTable: "discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "teacher_discipline_ibfk_2",
                table: "teacher_disciplines",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "activity_ibfk_2",
                table: "activity");

            migrationBuilder.DropForeignKey(
                name: "activity_protection_ibfk_1",
                table: "activity_protection");

            migrationBuilder.DropForeignKey(
                name: "activity_protection_ibfk_2",
                table: "activity_protection");

            migrationBuilder.DropForeignKey(
                name: "group_discipline_ibfk_1",
                table: "group_disciplines");

            migrationBuilder.DropForeignKey(
                name: "group_discipline_ibfk_2",
                table: "group_disciplines");

            migrationBuilder.DropForeignKey(
                name: "rating_control_ibfk_2",
                table: "rating_control");

            migrationBuilder.DropForeignKey(
                name: "rating_control_ibfk_3",
                table: "rating_control");

            migrationBuilder.DropForeignKey(
                name: "student_rating_ibfk_1",
                table: "student_rating");

            migrationBuilder.DropForeignKey(
                name: "student_rating_ibfk_2",
                table: "student_rating");

            migrationBuilder.DropForeignKey(
                name: "teacher_discipline_ibfk_1",
                table: "teacher_disciplines");

            migrationBuilder.DropForeignKey(
                name: "teacher_discipline_ibfk_2",
                table: "teacher_disciplines");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "specialty",
                type: "varchar(50)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 100,
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "academic_degree",
                table: "specialty",
                type: "enum('undergraduate','specialty','magistracy','postgraduate')",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "enum('Undergraduate','Specialty','Magistracy','Postgraduate')",
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "result",
                table: "graduation_work",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mark",
                table: "event_log",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "activity_ibfk_2",
                table: "activity",
                column: "semester_discipline_id",
                principalTable: "semester_discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "activity_protection_ibfk_1",
                table: "activity_protection",
                column: "student_id",
                principalTable: "student",
                principalColumn: "gradebook_number",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "activity_protection_ibfk_2",
                table: "activity_protection",
                column: "activity_id",
                principalTable: "activity",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "group_discipline_ibfk_1",
                table: "group_disciplines",
                column: "discipline_id",
                principalTable: "discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "group_discipline_ibfk_2",
                table: "group_disciplines",
                column: "group_id",
                principalTable: "academic_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "rating_control_ibfk_2",
                table: "rating_control",
                column: "semester_discipline_id",
                principalTable: "semester_discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "rating_control_ibfk_3",
                table: "rating_control",
                column: "group_id",
                principalTable: "academic_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "student_rating_ibfk_1",
                table: "student_rating",
                column: "student_id",
                principalTable: "student",
                principalColumn: "gradebook_number",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "student_rating_ibfk_2",
                table: "student_rating",
                column: "rating_id",
                principalTable: "rating_control",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "teacher_discipline_ibfk_1",
                table: "teacher_disciplines",
                column: "discipline_id",
                principalTable: "discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "teacher_discipline_ibfk_2",
                table: "teacher_disciplines",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
