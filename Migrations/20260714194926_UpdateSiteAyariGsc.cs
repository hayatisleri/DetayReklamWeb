using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalWebSitesi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSiteAyariGsc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LookerStudioUrl",
                table: "SiteAyarlari");

            migrationBuilder.AddColumn<string>(
                name: "GscPropertyUrl",
                table: "SiteAyarlari",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GscServiceAccountJsonPath",
                table: "SiteAyarlari",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GscPropertyUrl",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "GscServiceAccountJsonPath",
                table: "SiteAyarlari");

            migrationBuilder.AddColumn<string>(
                name: "LookerStudioUrl",
                table: "SiteAyarlari",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
