using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class Ratingcontrol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rating_control",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    semester_discipline_id = table.Column<int>(type: "int", nullable: false),
                    number = table.Column<int>(type: "int", nullable: false),
                    completion_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rating_control", x => x.id);
                    table.ForeignKey(
                        name: "rating_control_ibfk_2",
                        column: x => x.semester_discipline_id,
                        principalTable: "semester_discipline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_rating",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    rating_id = table.Column<int>(type: "int", nullable: false),
                    points = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_rating", x => x.id);
                    table.ForeignKey(
                        name: "student_rating_ibfk_1",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "gradebook_number",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "student_rating_ibfk_2",
                        column: x => x.rating_id,
                        principalTable: "rating_control",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "semester_discipline_id1",
                table: "rating_control",
                column: "semester_discipline_id");

            migrationBuilder.CreateIndex(
                name: "sr_rating_id",
                table: "student_rating",
                column: "rating_id");

            migrationBuilder.CreateIndex(
                name: "sr_student_id",
                table: "student_rating",
                column: "student_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_rating");

            migrationBuilder.DropTable(
                name: "rating_control");
        }
    }
}
