using Microsoft.EntityFrameworkCore.Migrations;

namespace RentalApp3.Migrations
{
    public partial class StatusInRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "rooms");
        }
    }
}
