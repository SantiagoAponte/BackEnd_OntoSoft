using Microsoft.EntityFrameworkCore.Migrations;

namespace persistence.Migrations
{
    public partial class Pruebas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserAppoinments",
                newName: "id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers",
                column: "typeDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserAppoinments",
                newName: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers",
                column: "typeDocumentId",
                unique: true,
                filter: "[typeDocumentId] IS NOT NULL");
        }
    }
}
