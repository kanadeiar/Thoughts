using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thoughts.DAL.SqlServer.Migrations.FileStorageDbMigrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Sha1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Counter = table.Column<int>(type: "int", nullable: false),
                    NameForDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNameForFileStorage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Md5 = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Meta = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Access = table.Column<byte>(type: "tinyint", nullable: false),
                    Flags = table.Column<byte>(type: "tinyint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Sha1);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_Md5_Meta_Sha1",
                table: "Files",
                columns: new[] { "Md5", "Meta", "Sha1" },
                unique: true,
                filter: "[Md5] IS NOT NULL AND [Meta] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
