using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class Requests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "teacher_request",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    graduation_work_id = table.Column<int>(type: "int", nullable: false),
                    teacher_id = table.Column<int>(type: "int", nullable: false),
                    motivation = table.Column<string>(type: "varchar(500)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    request_type = table.Column<string>(type: "enum('ADVISER','REVIEWER')", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "enum('APPROVED','REJECTED')", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reject_reason = table.Column<string>(type: "varchar(500)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    creating_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    response_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    response_person_name = table.Column<string>(type: "varchar(100)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_request", x => x.id);
                    table.ForeignKey(
                        name: "teacher_request_ibfk_1",
                        column: x => x.graduation_work_id,
                        principalTable: "graduation_work",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "teacher_request_ibfk_2",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "theme_request",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    graduation_work_id = table.Column<int>(type: "int", nullable: false),
                    theme = table.Column<string>(type: "varchar(150)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    motivation = table.Column<string>(type: "varchar(500)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    teacher_response = table.Column<string>(type: "enum('APPROVED','REJECTED')", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cafedra_response = table.Column<string>(type: "enum('APPROVED','REJECTED')", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "enum('APPROVED','REJECTED')", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reject_reason = table.Column<string>(type: "varchar(500)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    creating_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    response_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    response_person_name = table.Column<string>(type: "varchar(100)", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theme_request", x => x.id);
                    table.ForeignKey(
                        name: "theme_request_ibfk_1",
                        column: x => x.graduation_work_id,
                        principalTable: "graduation_work",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "graduation_work_id1",
                table: "teacher_request",
                column: "graduation_work_id");

            migrationBuilder.CreateIndex(
                name: "teacher_id1",
                table: "teacher_request",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "graduation_work_id2",
                table: "theme_request",
                column: "graduation_work_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "rating_control_ibfk_3",
                table: "rating_control");

            migrationBuilder.DropTable(
                name: "teacher_request");

            migrationBuilder.DropTable(
                name: "theme_request");

            migrationBuilder.DropIndex(
                name: "group_id1",
                table: "rating_control");

            migrationBuilder.DropIndex(
                name: "IX_rating_control_group_id",
                table: "rating_control");

            migrationBuilder.DropColumn(
                name: "group_id",
                table: "rating_control");

            migrationBuilder.RenameIndex(
                name: "group_id3",
                table: "student",
                newName: "group_id2");

            migrationBuilder.RenameIndex(
                name: "group_id2",
                table: "schedule_plan",
                newName: "group_id1");
        }
    }
}
