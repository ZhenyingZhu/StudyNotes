using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class AppTestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_AppTestModelAppTestInput",
                table: "AppTestChildModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppTestModel",
                table: "AppTestModel");

            migrationBuilder.DropIndex(
                name: "IX_AppTestChildModels_AppTestModelAppTestInput",
                table: "AppTestChildModels");

            migrationBuilder.DropColumn(
                name: "AppTestModelAppTestInput",
                table: "AppTestChildModels");

            migrationBuilder.AlterColumn<string>(
                name: "AppTestInput",
                table: "AppTestModel",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AppTestModel",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "AppTestModelId",
                table: "AppTestChildModels",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppTestModel",
                table: "AppTestModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppTestChildModels_AppTestModelId",
                table: "AppTestChildModels",
                column: "AppTestModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_AppTestModelId",
                table: "AppTestChildModels",
                column: "AppTestModelId",
                principalTable: "AppTestModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_AppTestModelId",
                table: "AppTestChildModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppTestModel",
                table: "AppTestModel");

            migrationBuilder.DropIndex(
                name: "IX_AppTestChildModels_AppTestModelId",
                table: "AppTestChildModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AppTestModel");

            migrationBuilder.DropColumn(
                name: "AppTestModelId",
                table: "AppTestChildModels");

            migrationBuilder.AlterColumn<string>(
                name: "AppTestInput",
                table: "AppTestModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppTestModelAppTestInput",
                table: "AppTestChildModels",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppTestModel",
                table: "AppTestModel",
                column: "AppTestInput");

            migrationBuilder.CreateIndex(
                name: "IX_AppTestChildModels_AppTestModelAppTestInput",
                table: "AppTestChildModels",
                column: "AppTestModelAppTestInput");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_AppTestModelAppTestInput",
                table: "AppTestChildModels",
                column: "AppTestModelAppTestInput",
                principalTable: "AppTestModel",
                principalColumn: "AppTestInput",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
