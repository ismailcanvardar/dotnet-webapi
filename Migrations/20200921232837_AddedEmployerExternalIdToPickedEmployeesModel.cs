using Microsoft.EntityFrameworkCore.Migrations;

namespace Commander.Migrations
{
    public partial class AddedEmployerExternalIdToPickedEmployeesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployerExternalId",
                table: "PickedEmployees",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerExternalId",
                table: "PickedEmployees");
        }
    }
}
