using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagementSystem.Migrations
{
    public partial class addObjArriveWating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkArrivedProcedureStatus",
                table: "WorkArrived",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkArriveWattings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkItemNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentTypeKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Subjective = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    KeyWord = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LeadershipDirectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Dealine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EvictionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserSign = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserCompile = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentCompile = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IndustryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferType = table.Column<int>(type: "int", nullable: false),
                    WorkflowStatus = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserIdCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIdUpdated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkArriveWattings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkArriveWattings");

            migrationBuilder.DropColumn(
                name: "WorkArrivedProcedureStatus",
                table: "WorkArrived");
        }
    }
}
