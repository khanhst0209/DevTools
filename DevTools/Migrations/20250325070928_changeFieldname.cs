using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeFieldname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugin_AspNetRoles_AccessiableRole",
                table: "Plugin");

            migrationBuilder.DropForeignKey(
                name: "FK_Plugin_PluginCategory_Categoryid",
                table: "Plugin");

            migrationBuilder.RenameColumn(
                name: "Categoryid",
                table: "Plugin",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Plugin",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AccessiableRole",
                table: "Plugin",
                newName: "AccessiableRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Plugin_Categoryid",
                table: "Plugin",
                newName: "IX_Plugin_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Plugin_AccessiableRole",
                table: "Plugin",
                newName: "IX_Plugin_AccessiableRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plugin_AspNetRoles_AccessiableRoleId",
                table: "Plugin",
                column: "AccessiableRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plugin_PluginCategory_CategoryId",
                table: "Plugin",
                column: "CategoryId",
                principalTable: "PluginCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugin_AspNetRoles_AccessiableRoleId",
                table: "Plugin");

            migrationBuilder.DropForeignKey(
                name: "FK_Plugin_PluginCategory_CategoryId",
                table: "Plugin");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Plugin",
                newName: "Categoryid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Plugin",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "AccessiableRoleId",
                table: "Plugin",
                newName: "AccessiableRole");

            migrationBuilder.RenameIndex(
                name: "IX_Plugin_CategoryId",
                table: "Plugin",
                newName: "IX_Plugin_Categoryid");

            migrationBuilder.RenameIndex(
                name: "IX_Plugin_AccessiableRoleId",
                table: "Plugin",
                newName: "IX_Plugin_AccessiableRole");

            migrationBuilder.AddForeignKey(
                name: "FK_Plugin_AspNetRoles_AccessiableRole",
                table: "Plugin",
                column: "AccessiableRole",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plugin_PluginCategory_Categoryid",
                table: "Plugin",
                column: "Categoryid",
                principalTable: "PluginCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
