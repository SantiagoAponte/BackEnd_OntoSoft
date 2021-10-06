using Microsoft.EntityFrameworkCore.Migrations;

namespace persistence.Migrations
{
    public partial class OntoSoftMigrationInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmergency",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "fullName",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fullName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmergency",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
