using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITTasks.Migrations
{
    /// <inheritdoc />
    public partial class Add_Roles_Seed_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ITRoles",
                columns: new[] { "Id", "CreateDate", "IsActive", "Type", "UpdateDate" },
                values: new object[,]
                {
                    { new Guid("7c290753-37f7-4887-b8eb-c93db8ed84b0"), new DateTime(2023, 6, 25, 14, 10, 2, 825, DateTimeKind.Local).AddTicks(6063), true, "user", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a37de7f7-834b-43bc-99f1-7369b5cdf317"), new DateTime(2023, 6, 25, 14, 10, 2, 825, DateTimeKind.Local).AddTicks(6047), true, "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ITRoles",
                keyColumn: "Id",
                keyValue: new Guid("7c290753-37f7-4887-b8eb-c93db8ed84b0"));

            migrationBuilder.DeleteData(
                table: "ITRoles",
                keyColumn: "Id",
                keyValue: new Guid("a37de7f7-834b-43bc-99f1-7369b5cdf317"));
        }
    }
}
