using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagementSystem.Migrations
{
    public partial class ChangeSomeFieldWorkItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Implementers",
                newName: "UserReceiveId");

            migrationBuilder.AlterColumn<Guid>(
                name: "IssuesId",
                table: "Implementers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentReceiveId",
                table: "Implementers",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentReceiveId",
                table: "Implementers");

            migrationBuilder.RenameColumn(
                name: "UserReceiveId",
                table: "Implementers",
                newName: "UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "IssuesId",
                table: "Implementers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
