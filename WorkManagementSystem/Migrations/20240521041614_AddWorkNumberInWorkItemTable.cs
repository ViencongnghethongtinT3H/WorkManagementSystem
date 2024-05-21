using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagementSystem.Migrations
{
    public partial class AddWorkNumberInWorkItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "WorkItems",
                newName: "IndustryId");

            migrationBuilder.AddColumn<string>(
                name: "WorkItemNumber",
                table: "WorkItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkItemNumber",
                table: "WorkItems");

            migrationBuilder.RenameColumn(
                name: "IndustryId",
                table: "WorkItems",
                newName: "CategoryId");
        }
    }
}
