using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace persistence.Migrations
{
    public partial class Cambioentabladeappoinments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "dateFinal",
                table: "Appoinments");

            migrationBuilder.DropColumn(
                name: "dateInit",
                table: "Appoinments");

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "Appoinments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers",
                column: "typeDocumentId",
                unique: true,
                filter: "[typeDocumentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "date",
                table: "Appoinments");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateFinal",
                table: "Appoinments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "dateInit",
                table: "Appoinments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers",
                column: "typeDocumentId");
        }
    }
}
