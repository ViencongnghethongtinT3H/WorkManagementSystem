using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagementSystem.Migrations
{
    public partial class CreateFilemanager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RefId",
                table: "FileAttachs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefId",
                table: "FileAttachs");
        }
    }
}
