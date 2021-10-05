using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class cambioEntidadAppoinments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appoinments_AspNetUsers_userId",
                table: "Appoinments");

            migrationBuilder.DropIndex(
                name: "IX_Appoinments_userId",
                table: "Appoinments");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Appoinments");

            migrationBuilder.CreateTable(
                name: "UserAppoinments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    AppoinmentsId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppoinments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAppoinments_Appoinments_AppoinmentsId",
                        column: x => x.AppoinmentsId,
                        principalTable: "Appoinments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAppoinments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAppoinments_AppoinmentsId",
                table: "UserAppoinments",
                column: "AppoinmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppoinments_UserId",
                table: "UserAppoinments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAppoinments");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Appoinments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appoinments_userId",
                table: "Appoinments",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appoinments_AspNetUsers_userId",
                table: "Appoinments",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
