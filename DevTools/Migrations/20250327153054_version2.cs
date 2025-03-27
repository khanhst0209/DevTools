using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class version2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPlugins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PluginId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlugins", x => new { x.UserId, x.PluginId });
                    table.ForeignKey(
                        name: "FK_UserPlugins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlugins_Plugin_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlugins_PluginId",
                table: "UserPlugins",
                column: "PluginId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPlugins");
        }
    }
}
