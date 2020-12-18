using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class GroupDisciplines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "group_id1",
                table: "student",
                newName: "group_id2");

            migrationBuilder.RenameIndex(
                name: "group_id",
                table: "schedule_plan",
                newName: "group_id1");

            migrationBuilder.RenameColumn(
                name: "discipline_id",
                table: "activity",
                newName: "semester_discipline_id");

            migrationBuilder.RenameIndex(
                name: "discipline_id",
                table: "activity",
                newName: "semester_discipline_id");

            migrationBuilder.CreateTable(
                name: "group_disciplines",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    group_id = table.Column<int>(type: "int", nullable: false),
                    discipline_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_disciplines", x => x.id);
                    table.ForeignKey(
                        name: "group_discipline_ibfk_1",
                        column: x => x.discipline_id,
                        principalTable: "discipline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "group_discipline_ibfk_2",
                        column: x => x.group_id,
                        principalTable: "academic_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "discipline_id",
                table: "group_disciplines",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "group_id",
                table: "group_disciplines",
                column: "group_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_disciplines");

            migrationBuilder.RenameIndex(
                name: "group_id2",
                table: "student",
                newName: "group_id1");

            migrationBuilder.RenameIndex(
                name: "group_id1",
                table: "schedule_plan",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "semester_discipline_id",
                table: "activity",
                newName: "discipline_id");

            migrationBuilder.RenameIndex(
                name: "semester_discipline_id",
                table: "activity",
                newName: "discipline_id");
        }
    }
}
