using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class crearEntidad_FaceTooth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "faceToothId",
                table: "typeProcessTooth",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "faceTooth",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faceTooth", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_typeProcessTooth_faceToothId",
                table: "typeProcessTooth",
                column: "faceToothId");

            migrationBuilder.AddForeignKey(
                name: "FK_typeProcessTooth_faceTooth_faceToothId",
                table: "typeProcessTooth",
                column: "faceToothId",
                principalTable: "faceTooth",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_typeProcessTooth_faceTooth_faceToothId",
                table: "typeProcessTooth");

            migrationBuilder.DropTable(
                name: "faceTooth");

            migrationBuilder.DropIndex(
                name: "IX_typeProcessTooth_faceToothId",
                table: "typeProcessTooth");

            migrationBuilder.DropColumn(
                name: "faceToothId",
                table: "typeProcessTooth");
        }
    }
}
