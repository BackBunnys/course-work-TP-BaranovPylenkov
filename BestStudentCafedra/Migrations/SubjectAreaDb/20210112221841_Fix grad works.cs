using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class Fixgradworks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_graduation_work_teacher_scientific_adviser_id",
                table: "graduation_work");

            migrationBuilder.DropForeignKey(
                name: "graduation_works_ibfk_3",
                table: "graduation_work");

            migrationBuilder.AddForeignKey(
                name: "graduation_works_ibfk_2",
                table: "graduation_work",
                column: "scientific_adviser_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "graduation_works_ibfk_3",
                table: "graduation_work",
                column: "reviewer_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "graduation_works_ibfk_2",
                table: "graduation_work");

            migrationBuilder.DropForeignKey(
                name: "graduation_works_ibfk_3",
                table: "graduation_work");

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
    }
}
