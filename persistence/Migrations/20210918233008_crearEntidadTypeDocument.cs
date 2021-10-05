using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class crearEntidadTypeDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "addresContact",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bloodType",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "centerEmergency",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contactEmergency",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateBirth",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "document",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "eps",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "height",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phoneEmergency",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rh",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "typeDocumentId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "weight",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "typeDocument",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeDocument", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers",
                column: "typeDocumentId",
                unique: true,
                filter: "[typeDocumentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_typeDocument_typeDocumentId",
                table: "AspNetUsers",
                column: "typeDocumentId",
                principalTable: "typeDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_typeDocument_typeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "typeDocument");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "addresContact",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "bloodType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "centerEmergency",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "city",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "contactEmergency",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "dateBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "document",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "eps",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "height",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "phoneEmergency",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "rh",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "typeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "weight",
                table: "AspNetUsers");
        }
    }
}
