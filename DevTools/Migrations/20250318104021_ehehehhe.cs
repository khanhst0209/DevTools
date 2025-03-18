using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ehehehhe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Plugin_Categoryid",
                table: "Plugin",
                column: "Categoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_Plugin_PluginCategory_Categoryid",
                table: "Plugin",
                column: "Categoryid",
                principalTable: "PluginCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugin_PluginCategory_Categoryid",
                table: "Plugin");

            migrationBuilder.DropIndex(
                name: "IX_Plugin_Categoryid",
                table: "Plugin");
        }
    }
}
