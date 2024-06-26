using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagementSystem.Migrations
{
    public partial class addAccountReceiveIdForReceiveCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiveCompanyId",
                table: "DispatchReceiveCompanies",
                newName: "AccountReceiveId");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountReceiveId",
                table: "ReceiveCompanys",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountReceiveId",
                table: "ReceiveCompanys");

            migrationBuilder.RenameColumn(
                name: "AccountReceiveId",
                table: "DispatchReceiveCompanies",
                newName: "ReceiveCompanyId");
        }
    }
}
