using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class Semesterdisciplines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "activity_ibfk_2",
                table: "activity");

            migrationBuilder.DropColumn(
                name: "control_type",
                table: "discipline");

            migrationBuilder.DropColumn(
                name: "semester",
                table: "discipline");

            migrationBuilder.DropColumn(
                name: "year",
                table: "discipline");

            migrationBuilder.RenameIndex(
                name: "discipline_id1",
                table: "teacher_disciplines",
                newName: "discipline_id2");

            migrationBuilder.CreateTable(
                name: "semester_discipline",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    discipline_id = table.Column<int>(type: "int", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    semester = table.Column<int>(type: "int", nullable: false),
                    control_type = table.Column<string>(type: "enum('exam','differential credit','credit')", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semester_discipline", x => x.id);
                    table.ForeignKey(
                        name: "semester_discipline_ibfk_1",
                        column: x => x.discipline_id,
                        principalTable: "discipline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "discipline_id1",
                table: "semester_discipline",
                column: "discipline_id");

            migrationBuilder.AddForeignKey(
                name: "activity_ibfk_2",
                table: "activity",
                column: "discipline_id",
                principalTable: "semester_discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "activity_ibfk_2",
                table: "activity");

            migrationBuilder.DropTable(
                name: "semester_discipline");

            migrationBuilder.RenameIndex(
                name: "discipline_id2",
                table: "teacher_disciplines",
                newName: "discipline_id1");

            migrationBuilder.AddColumn<string>(
                name: "control_type",
                table: "discipline",
                type: "enum('exam','differential credit','credit')",
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "semester",
                table: "discipline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "year",
                table: "discipline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "activity_ibfk_2",
                table: "activity",
                column: "discipline_id",
                principalTable: "discipline",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
