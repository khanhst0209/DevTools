using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "concacnho");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a2b3c4d-1234-5678-9abc-def123456789");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b3c4d5e-2345-6789-abcd-ef1234567890");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "aa24b563-3c1d-41f2-91ad-08d3b2f8e63c", null, "User", "USER" },
                    { "c1e2bcd1-5f2b-4ad8-b8d5-08d3b2f8e63b", null, "Admin", "ADMIN" },
                    { "f3b87c41-1f6d-4a2f-8d1a-08d3b2f8e63d", null, "Premium", "PREMIUM" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa24b563-3c1d-41f2-91ad-08d3b2f8e63c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1e2bcd1-5f2b-4ad8-b8d5-08d3b2f8e63b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3b87c41-1f6d-4a2f-8d1a-08d3b2f8e63d");

            migrationBuilder.CreateTable(
                name: "concacnho",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_concacnho", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a2b3c4d-1234-5678-9abc-def123456789", null, "Admin", "ADMIN" },
                    { "2b3c4d5e-2345-6789-abcd-ef1234567890", null, "User", "USER" }
                });
        }
    }
}
