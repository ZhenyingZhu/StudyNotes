using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppTestInput",
                table: "AppTestModel",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AppTestModel",
                columns: new[] { "Id", "AppTestInput" },
                values: new object[] { 10, "Seeding Test1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppTestModel",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.AlterColumn<string>(
                name: "AppTestInput",
                table: "AppTestModel",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
