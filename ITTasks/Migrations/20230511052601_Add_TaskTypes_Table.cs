using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITTasks.Migrations
{
    /// <inheritdoc />
    public partial class Add_TaskTypes_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ITTaskTypeId",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TasksType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ITTaskTypeId",
                table: "Tasks",
                column: "ITTaskTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TasksType_ITTaskTypeId",
                table: "Tasks",
                column: "ITTaskTypeId",
                principalTable: "TasksType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TasksType_ITTaskTypeId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "TasksType");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ITTaskTypeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ITTaskTypeId",
                table: "Tasks");
        }
    }
}
