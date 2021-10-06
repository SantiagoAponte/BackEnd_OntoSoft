using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace persistence.Migrations
{
    public partial class crearEntidadesClinicHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "clinicHistoryId",
                table: "Odontogram",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "backgroundMedicals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_backgroundMedicals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "backgroundOrals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_backgroundOrals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "clinicHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    phoneCompanion = table.Column<string>(nullable: true),
                    nameCompanion = table.Column<string>(nullable: true),
                    dateRegister = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    backgroundMedical = table.Column<bool>(nullable: false),
                    backgroundOral = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinicHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_clinicHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "backgroundMedicalClinicHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    clinicHistoryId = table.Column<Guid>(nullable: false),
                    BackgroundMedicalsId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_backgroundMedicalClinicHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_backgroundMedicalClinicHistories_backgroundMedicals_BackgroundMedicalsId",
                        column: x => x.BackgroundMedicalsId,
                        principalTable: "backgroundMedicals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_backgroundMedicalClinicHistories_clinicHistories_clinicHistoryId",
                        column: x => x.clinicHistoryId,
                        principalTable: "clinicHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "backgroundOralClinicHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    clinicHistoryId = table.Column<Guid>(nullable: false),
                    BackgroundOralsId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_backgroundOralClinicHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_backgroundOralClinicHistories_backgroundOrals_BackgroundOralsId",
                        column: x => x.BackgroundOralsId,
                        principalTable: "backgroundOrals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_backgroundOralClinicHistories_clinicHistories_clinicHistoryId",
                        column: x => x.clinicHistoryId,
                        principalTable: "clinicHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "oralRadiography",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    dateRegister = table.Column<DateTime>(nullable: false),
                    observation = table.Column<string>(nullable: true),
                    clinicHistoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oralRadiography", x => x.Id);
                    table.ForeignKey(
                        name: "FK_oralRadiography_clinicHistories_clinicHistoryId",
                        column: x => x.clinicHistoryId,
                        principalTable: "clinicHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "patientEvolution",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    observation = table.Column<string>(nullable: true),
                    dateCreate = table.Column<DateTime>(nullable: false),
                    clinicHistoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientEvolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_patientEvolution_clinicHistories_clinicHistoryId",
                        column: x => x.clinicHistoryId,
                        principalTable: "clinicHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "treamentPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    observation = table.Column<string>(nullable: true),
                    clinicHistoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_treamentPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_treamentPlan_clinicHistories_clinicHistoryId",
                        column: x => x.clinicHistoryId,
                        principalTable: "clinicHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Odontogram_clinicHistoryId",
                table: "Odontogram",
                column: "clinicHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_backgroundMedicalClinicHistories_BackgroundMedicalsId",
                table: "backgroundMedicalClinicHistories",
                column: "BackgroundMedicalsId");

            migrationBuilder.CreateIndex(
                name: "IX_backgroundMedicalClinicHistories_clinicHistoryId",
                table: "backgroundMedicalClinicHistories",
                column: "clinicHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_backgroundOralClinicHistories_BackgroundOralsId",
                table: "backgroundOralClinicHistories",
                column: "BackgroundOralsId");

            migrationBuilder.CreateIndex(
                name: "IX_backgroundOralClinicHistories_clinicHistoryId",
                table: "backgroundOralClinicHistories",
                column: "clinicHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_clinicHistories_UserId",
                table: "clinicHistories",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_oralRadiography_clinicHistoryId",
                table: "oralRadiography",
                column: "clinicHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_patientEvolution_clinicHistoryId",
                table: "patientEvolution",
                column: "clinicHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_treamentPlan_clinicHistoryId",
                table: "treamentPlan",
                column: "clinicHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Odontogram_clinicHistories_clinicHistoryId",
                table: "Odontogram",
                column: "clinicHistoryId",
                principalTable: "clinicHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odontogram_clinicHistories_clinicHistoryId",
                table: "Odontogram");

            migrationBuilder.DropTable(
                name: "backgroundMedicalClinicHistories");

            migrationBuilder.DropTable(
                name: "backgroundOralClinicHistories");

            migrationBuilder.DropTable(
                name: "oralRadiography");

            migrationBuilder.DropTable(
                name: "patientEvolution");

            migrationBuilder.DropTable(
                name: "treamentPlan");

            migrationBuilder.DropTable(
                name: "backgroundMedicals");

            migrationBuilder.DropTable(
                name: "backgroundOrals");

            migrationBuilder.DropTable(
                name: "clinicHistories");

            migrationBuilder.DropIndex(
                name: "IX_Odontogram_clinicHistoryId",
                table: "Odontogram");

            migrationBuilder.DropColumn(
                name: "clinicHistoryId",
                table: "Odontogram");
        }
    }
}
