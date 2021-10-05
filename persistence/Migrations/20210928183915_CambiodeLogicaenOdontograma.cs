using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class CambiodeLogicaenOdontograma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "toothsOdontogram");

            migrationBuilder.DropIndex(
                name: "IX_Odontogram_UserId",
                table: "Odontogram");

            migrationBuilder.DropIndex(
                name: "IX_clinicHistories_UserId",
                table: "clinicHistories");

            migrationBuilder.AddColumn<Guid>(
                name: "OdontogramId",
                table: "typeProcessTooth",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_typeProcessTooth_OdontogramId",
                table: "typeProcessTooth",
                column: "OdontogramId");

            migrationBuilder.CreateIndex(
                name: "IX_Odontogram_UserId",
                table: "Odontogram",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_clinicHistories_UserId",
                table: "clinicHistories",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_typeProcessTooth_Odontogram_OdontogramId",
                table: "typeProcessTooth",
                column: "OdontogramId",
                principalTable: "Odontogram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_typeProcessTooth_Odontogram_OdontogramId",
                table: "typeProcessTooth");

            migrationBuilder.DropIndex(
                name: "IX_typeProcessTooth_OdontogramId",
                table: "typeProcessTooth");

            migrationBuilder.DropIndex(
                name: "IX_Odontogram_UserId",
                table: "Odontogram");

            migrationBuilder.DropIndex(
                name: "IX_clinicHistories_UserId",
                table: "clinicHistories");

            migrationBuilder.DropColumn(
                name: "OdontogramId",
                table: "typeProcessTooth");

            migrationBuilder.CreateTable(
                name: "toothsOdontogram",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OdontogramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToothId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_toothsOdontogram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_toothsOdontogram_Odontogram_OdontogramId",
                        column: x => x.OdontogramId,
                        principalTable: "Odontogram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_toothsOdontogram_tooth_ToothId",
                        column: x => x.ToothId,
                        principalTable: "tooth",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Odontogram_UserId",
                table: "Odontogram",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_clinicHistories_UserId",
                table: "clinicHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_toothsOdontogram_OdontogramId",
                table: "toothsOdontogram",
                column: "OdontogramId");

            migrationBuilder.CreateIndex(
                name: "IX_toothsOdontogram_ToothId",
                table: "toothsOdontogram",
                column: "ToothId");
        }
    }
}
