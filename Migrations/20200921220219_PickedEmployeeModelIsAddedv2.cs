using Microsoft.EntityFrameworkCore.Migrations;

namespace Commander.Migrations
{
    public partial class PickedEmployeeModelIsAddedv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickedEmployeeExternalId",
                table: "PickedEmployees");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeExternalId",
                table: "PickedEmployees",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeExternalId",
                table: "PickedEmployees");

            migrationBuilder.AddColumn<string>(
                name: "PickedEmployeeExternalId",
                table: "PickedEmployees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
