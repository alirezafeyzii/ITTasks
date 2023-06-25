using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITTasks.Migrations
{
    /// <inheritdoc />
    public partial class Chnage_Type_PhoneNumber_From_int_To_string : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ITRoles",
                keyColumn: "Id",
                keyValue: new Guid("7c290753-37f7-4887-b8eb-c93db8ed84b0"));

            migrationBuilder.DeleteData(
                table: "ITRoles",
                keyColumn: "Id",
                keyValue: new Guid("a37de7f7-834b-43bc-99f1-7369b5cdf317"));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.InsertData(
                table: "ITRoles",
                columns: new[] { "Id", "CreateDate", "IsActive", "Type", "UpdateDate" },
                values: new object[,]
                {
                    { new Guid("3f6b797b-9a31-4956-9250-6698e335ba97"), new DateTime(2023, 6, 25, 15, 54, 4, 321, DateTimeKind.Local).AddTicks(3987), true, "user", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d8b99c9e-518d-4ffd-b211-cd36fc2afb22"), new DateTime(2023, 6, 25, 15, 54, 4, 321, DateTimeKind.Local).AddTicks(3971), true, "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ITRoles",
                keyColumn: "Id",
                keyValue: new Guid("3f6b797b-9a31-4956-9250-6698e335ba97"));

            migrationBuilder.DeleteData(
                table: "ITRoles",
                keyColumn: "Id",
                keyValue: new Guid("d8b99c9e-518d-4ffd-b211-cd36fc2afb22"));

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "ITRoles",
                columns: new[] { "Id", "CreateDate", "IsActive", "Type", "UpdateDate" },
                values: new object[,]
                {
                    { new Guid("7c290753-37f7-4887-b8eb-c93db8ed84b0"), new DateTime(2023, 6, 25, 14, 10, 2, 825, DateTimeKind.Local).AddTicks(6063), true, "user", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a37de7f7-834b-43bc-99f1-7369b5cdf317"), new DateTime(2023, 6, 25, 14, 10, 2, 825, DateTimeKind.Local).AddTicks(6047), true, "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
