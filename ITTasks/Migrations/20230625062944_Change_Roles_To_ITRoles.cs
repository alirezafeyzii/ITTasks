using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITTasks.Migrations
{
    /// <inheritdoc />
    public partial class Change_Roles_To_ITRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "ITRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ITRoles",
                table: "ITRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ITRoles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "ITRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ITRoles_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ITRoles",
                table: "ITRoles");

            migrationBuilder.RenameTable(
                name: "ITRoles",
                newName: "Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
