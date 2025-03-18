using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class lasthehe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1e2bcd1-5f2b-4ad8-b8d5-08d3b2f81231",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Anonymous", "ANONYMOUS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1e2bcd1-5f2b-4ad8-b8d5-08d3b2f81231",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Anounymous", "ANOUNYMOUS" });
        }
    }
}
