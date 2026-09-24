using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalWebSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AddWhatsappNumarasiToSiteAyar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhatsappNumarasi",
                table: "SiteAyarlari",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatsappNumarasi",
                table: "SiteAyarlari");
        }
    }
}
