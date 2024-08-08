using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.DAL.Repository.Migrations
{
    public partial class add_column_test_to_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Users");
        }
    }
}
