using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagementSystem.Migrations
{
    public partial class AddWorkArrived : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormInput",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "DateArrival",
                table: "WorkArrived");

            migrationBuilder.DropColumn(
                name: "EvictionTime",
                table: "WorkArrived");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "WorkArrived");

            migrationBuilder.DropColumn(
                name: "KeyWord",
                table: "WorkArrived");

            migrationBuilder.DropColumn(
                name: "Subjective",
                table: "WorkArrived");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkArrived");

            migrationBuilder.RenameColumn(
                name: "ProcessingStatus",
                table: "WorkArrived",
                newName: "WorkArrivedStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowId",
                table: "WorkflowSteps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TransferType",
                table: "WorkArrived",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "TransferType",
                table: "WorkArrived");

            migrationBuilder.RenameColumn(
                name: "WorkArrivedStatus",
                table: "WorkArrived",
                newName: "ProcessingStatus");

            migrationBuilder.AddColumn<string>(
                name: "FormInput",
                table: "WorkflowSteps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateArrival",
                table: "WorkArrived",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EvictionTime",
                table: "WorkArrived",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IndustryId",
                table: "WorkArrived",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyWord",
                table: "WorkArrived",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subjective",
                table: "WorkArrived",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "WorkArrived",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
