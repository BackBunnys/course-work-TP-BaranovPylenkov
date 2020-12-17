using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class EventTemplatefixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "schedule_plan_event_id",
                table: "event_log",
                newName: "event_id");

            migrationBuilder.RenameIndex(
                name: "schedule_plan_event_id",
                table: "event_log",
                newName: "event_id");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "event_template",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true,
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "event_id",
                table: "event_log",
                newName: "schedule_plan_event_id");

            migrationBuilder.RenameIndex(
                name: "event_id",
                table: "event_log",
                newName: "schedule_plan_event_id");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "event_template",
                type: "varchar(150)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldCollation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
