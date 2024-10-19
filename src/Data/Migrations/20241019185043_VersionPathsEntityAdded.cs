using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class VersionPathsEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Paths",
                columns: table => new
                {
                    VersionInfoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChangelogPath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ZipPath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paths", x => x.VersionInfoId);
                    table.ForeignKey(
                        name: "FK_Paths_Versions_VersionInfoId",
                        column: x => x.VersionInfoId,
                        principalTable: "Versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paths");
        }
    }
}
