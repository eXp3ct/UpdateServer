using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ForegingKeyAddedToVersionInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Versions_ApplicationId",
                table: "Versions",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Versions_Applications_ApplicationId",
                table: "Versions",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Versions_Applications_ApplicationId",
                table: "Versions");

            migrationBuilder.DropIndex(
                name: "IX_Versions_ApplicationId",
                table: "Versions");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Versions");
        }
    }
}