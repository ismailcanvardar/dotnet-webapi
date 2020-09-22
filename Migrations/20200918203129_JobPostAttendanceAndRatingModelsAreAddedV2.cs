using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Commander.Migrations
{
    public partial class JobPostAttendanceAndRatingModelsAreAddedV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(nullable: false),
                    EmployerId = table.Column<string>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: false),
                    JobPostId = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobPosts",
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
                    table.PrimaryKey("PK_JobPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(nullable: false),
                    EmployerId = table.Column<string>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "JobPosts");

            migrationBuilder.DropTable(
                name: "Ratings");
        }
    }
}
