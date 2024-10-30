using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEntityReleations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Versions_Paths_PathId",
                table: "Versions");

            migrationBuilder.DropIndex(
                name: "IX_Versions_PathId",
                table: "Versions");

            migrationBuilder.DropColumn(
                name: "PathId",
                table: "Versions");

            migrationBuilder.AddColumn<int>(
                name: "VersionInfoId",
                table: "Paths",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Paths_VersionInfoId",
                table: "Paths",
                column: "VersionInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Paths_Versions_VersionInfoId",
                table: "Paths",
                column: "VersionInfoId",
                principalTable: "Versions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paths_Versions_VersionInfoId",
                table: "Paths");

            migrationBuilder.DropIndex(
                name: "IX_Paths_VersionInfoId",
                table: "Paths");

            migrationBuilder.DropColumn(
                name: "VersionInfoId",
                table: "Paths");

            migrationBuilder.AddColumn<int>(
                name: "PathId",
                table: "Versions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Versions_PathId",
                table: "Versions",
                column: "PathId");

            migrationBuilder.AddForeignKey(
                name: "FK_Versions_Paths_PathId",
                table: "Versions",
                column: "PathId",
                principalTable: "Paths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
