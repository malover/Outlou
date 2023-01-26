using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutlouTest.Migrations
{
    public partial class SummaryAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "FeedItems",
                newName: "Summary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "FeedItems",
                newName: "PublishDate");
        }
    }
}
