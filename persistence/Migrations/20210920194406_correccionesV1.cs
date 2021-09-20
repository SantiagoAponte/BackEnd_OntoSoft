using Microsoft.EntityFrameworkCore.Migrations;

namespace persistence.Migrations
{
    public partial class correccionesV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clinicHistories_UserId",
                table: "clinicHistories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_clinicHistories_UserId",
                table: "clinicHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers",
                column: "typeDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clinicHistories_UserId",
                table: "clinicHistories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_clinicHistories_UserId",
                table: "clinicHistories",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_typeDocumentId",
                table: "AspNetUsers",
                column: "typeDocumentId",
                unique: true,
                filter: "[typeDocumentId] IS NOT NULL");
        }
    }
}
