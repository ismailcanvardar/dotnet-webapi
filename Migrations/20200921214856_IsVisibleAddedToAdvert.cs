using Microsoft.EntityFrameworkCore.Migrations;

namespace Commander.Migrations
{
    public partial class IsVisibleAddedToAdvert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalParticipantCount",
                table: "Adverts");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Adverts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalApplicantCount",
                table: "Adverts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "TotalApplicantCount",
                table: "Adverts");

            migrationBuilder.AddColumn<int>(
                name: "TotalParticipantCount",
                table: "Adverts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
