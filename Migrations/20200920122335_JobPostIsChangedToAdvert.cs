using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Commander.Migrations
{
    public partial class JobPostIsChangedToAdvert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobPosts");

            migrationBuilder.DropColumn(
                name: "JobPostId",
                table: "Attendances");

            migrationBuilder.AddColumn<string>(
                name: "AdvertId",
                table: "Attendances",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Adverts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(nullable: false),
                    PostOwner = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    DailySalary = table.Column<float>(nullable: false),
                    NeededEmployee = table.Column<int>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Neighborhood = table.Column<string>(nullable: false),
                    Thumbnail = table.Column<string>(nullable: true),
                    TotalParticipantCount = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adverts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adverts");

            migrationBuilder.DropColumn(
                name: "AdvertId",
                table: "Attendances");

            migrationBuilder.AddColumn<string>(
                name: "JobPostId",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "JobPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DailySalary = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NeededEmployee = table.Column<int>(type: "int", nullable: false),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostOwner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalParticipantCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPosts", x => x.Id);
                });
        }
    }
}
