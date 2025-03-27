using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa24b563-3c1d-41f2-91ad-08d3b2f8e63c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1e2bcd1-5f2b-4ad8-b8d5-08d3b2f81231");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1e2bcd1-5f2b-4ad8-b8d5-08d3b2f8e63b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3b87c41-1f6d-4a2f-8d1a-08d3b2f8e63d");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "Name");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "aa24b563-3c1d-41f2-91ad-08d3b2f8e63c", null, "User", "USER" },
                    { "c1e2bcd1-5f2b-4ad8-b8d5-08d3b2f81231", null, "Anonymous", "ANONYMOUS" },
                    { "c1e2bcd1-5f2b-4ad8-b8d5-08d3b2f8e63b", null, "Admin", "ADMIN" },
                    { "f3b87c41-1f6d-4a2f-8d1a-08d3b2f8e63d", null, "Premium", "PREMIUM" }
                });
        }
    }
}
