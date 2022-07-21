using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fair.Company.Infrastructure.SqlServer.Migrations
{
    public partial class UpdateCompanyAddContactData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookAddress",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramAddress",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber1",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber2",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterAddress",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeAddress",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "FacebookAddress",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "InstagramAddress",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LegalName",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PhoneNumber1",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PhoneNumber2",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "TwitterAddress",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "YoutubeAddress",
                table: "Company");
        }
    }
}
