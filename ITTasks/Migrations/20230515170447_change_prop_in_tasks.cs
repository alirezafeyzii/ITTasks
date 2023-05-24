using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITTasks.Migrations
{
    /// <inheritdoc />
    public partial class change_prop_in_tasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourAmount",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Tasks");

            migrationBuilder.AddColumn<float>(
                name: "HourAmount",
                table: "Tasks",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
