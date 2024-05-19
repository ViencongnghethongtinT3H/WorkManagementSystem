using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDocumentTypeKeyInWorkItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "WorkItems");

            migrationBuilder.AddColumn<string>(
                name: "DocumentTypeKey",
                table: "WorkItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentTypeKey",
                table: "WorkItems");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "WorkItems",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
