using Microsoft.EntityFrameworkCore.Migrations;

namespace BestStudentCafedra.Migrations.SubjectAreaDb
{
    public partial class DataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "activity_type",
                columns: new[] { "name" },
                values: new object[] { "Лабораторная работа" }
            );

            migrationBuilder.InsertData(
                table: "activity_type",
                columns: new[] { "name" },
                values: new object[] { "Практическая работа" }
            );

            migrationBuilder.InsertData(
                table: "activity_type",
                columns: new[] { "name" },
                values: new object[] { "Самостоятельная работа" }
            );

            migrationBuilder.InsertData(
                table: "activity_type",
                columns: new[] { "name" },
                values: new object[] { "Контрольная работа" }
            );

            migrationBuilder.InsertData(
                table: "activity_type",
                columns: new[] { "name" },
                values: new object[] { "Экзамен" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "activity_type",
                keyColumns: new[] { "name" },
                keyValues: new object[] { "Лабораторная работа" }
            );

            migrationBuilder.DeleteData(
                table: "activity_type",
                keyColumns: new[] { "name" },
                keyValues: new object[] { "Практическая работа'" }
            );

            migrationBuilder.DeleteData(
                table: "activity_type",
                keyColumns: new[] { "name" },
                keyValues: new object[] { "Самостоятельная работа" }
            );

            migrationBuilder.DeleteData(
                table: "activity_type",
                keyColumns: new[] { "name" },
                keyValues: new object[] { "Контрольная работа" }
            );

            migrationBuilder.DeleteData(
                table: "activity_type",
                keyColumns: new[] { "name" },
                keyValues: new object[] { "Экзамен" }
            );
        }
    }
}
