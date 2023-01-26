using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutlouTest.Migrations
{
    public partial class FixIsReadProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "FeedItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "FeedItems");
        }
    }
}
