using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace persistence.Migrations
{
    public partial class crearEntidadesOdontograma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Odontogram",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    date_register = table.Column<DateTime>(nullable: false),
                    observation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odontogram", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tooth",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    num = table.Column<int>(nullable: false),
                    ubicacion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tooth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "typeProcess",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeProcess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userOdontogram",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    OdontogramId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userOdontogram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userOdontogram_Odontogram_OdontogramId",
                        column: x => x.OdontogramId,
                        principalTable: "Odontogram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userOdontogram_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "toothsOdontogram",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OdontogramId = table.Column<Guid>(nullable: false),
                    ToothId = table.Column<Guid>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "typeProcessOdontogram",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OdontogramId = table.Column<Guid>(nullable: false),
                    typeProcessId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeProcessOdontogram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_typeProcessOdontogram_Odontogram_OdontogramId",
                        column: x => x.OdontogramId,
                        principalTable: "Odontogram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_typeProcessOdontogram_typeProcess_typeProcessId",
                        column: x => x.typeProcessId,
                        principalTable: "typeProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_toothsOdontogram_OdontogramId",
                table: "toothsOdontogram",
                column: "OdontogramId");

            migrationBuilder.CreateIndex(
                name: "IX_toothsOdontogram_ToothId",
                table: "toothsOdontogram",
                column: "ToothId");

            migrationBuilder.CreateIndex(
                name: "IX_typeProcessOdontogram_OdontogramId",
                table: "typeProcessOdontogram",
                column: "OdontogramId");

            migrationBuilder.CreateIndex(
                name: "IX_typeProcessOdontogram_typeProcessId",
                table: "typeProcessOdontogram",
                column: "typeProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_userOdontogram_OdontogramId",
                table: "userOdontogram",
                column: "OdontogramId");

            migrationBuilder.CreateIndex(
                name: "IX_userOdontogram_UserId",
                table: "userOdontogram",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "toothsOdontogram");

            migrationBuilder.DropTable(
                name: "typeProcessOdontogram");

            migrationBuilder.DropTable(
                name: "userOdontogram");

            migrationBuilder.DropTable(
                name: "tooth");

            migrationBuilder.DropTable(
                name: "typeProcess");

            migrationBuilder.DropTable(
                name: "Odontogram");
        }
    }
}
