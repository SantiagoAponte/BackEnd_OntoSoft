using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace persistence.Migrations
{
    public partial class sequitarelacionentreodontogramayhistoriaclinica : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odontogram_clinicHistories_clinicHistoryId",
                table: "Odontogram");

            migrationBuilder.DropIndex(
                name: "IX_Odontogram_clinicHistoryId",
                table: "Odontogram");

            migrationBuilder.DropColumn(
                name: "clinicHistoryId",
                table: "Odontogram");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "clinicHistoryId",
                table: "Odontogram",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Odontogram_clinicHistoryId",
                table: "Odontogram",
                column: "clinicHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Odontogram_clinicHistories_clinicHistoryId",
                table: "Odontogram",
                column: "clinicHistoryId",
                principalTable: "clinicHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
