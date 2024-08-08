using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.DAL.Repository.Migrations
{
    public partial class delete_test_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
