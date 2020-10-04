using Microsoft.EntityFrameworkCore.Migrations;

namespace KariyerAppApi.Migrations
{
    public partial class NewFieldsToEmployerAndEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CampaignAllowance",
                table: "Employers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Employers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "KvkkAgreement",
                table: "Employers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Employers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "UserAgreement",
                table: "Employers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CampaignAllowance",
                table: "Employees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Employees",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "KvkkAgreement",
                table: "Employees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Employees",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "UserAgreement",
                table: "Employees",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampaignAllowance",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "KvkkAgreement",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "UserAgreement",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "CampaignAllowance",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "KvkkAgreement",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserAgreement",
                table: "Employees");
        }
    }
}
