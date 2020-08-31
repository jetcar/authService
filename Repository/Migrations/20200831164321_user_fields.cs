using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class user_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "users",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "users",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "personal_id",
                table: "users",
                maxLength: 36,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "personal_id",
                table: "users");
        }
    }
}