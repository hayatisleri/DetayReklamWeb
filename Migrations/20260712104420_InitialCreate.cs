using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalWebSitesi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Haberler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SeoUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Ozet = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResimAdresi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Haberler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hizmetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KisaAciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResimAdresi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hizmetler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IletisimMesajlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Eposta = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Konu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OkunduMu = table.Column<bool>(type: "bit", nullable: false),
                    GonderilmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IletisimMesajlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sayfalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SeoUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    SeoBasligi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SeoAciklamasi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sayfalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteAyarlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteBasligi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SiteAciklamasi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LogoAdresi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FaviconAdresi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TelefonNumarasi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EpostaAdresi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FacebookAdresi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InstagramAdresi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LinkedInAdresi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAyarlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slaytlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AltBaslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ResimAdresi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LinkAdresi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slaytlar", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Haberler");

            migrationBuilder.DropTable(
                name: "Hizmetler");

            migrationBuilder.DropTable(
                name: "IletisimMesajlari");

            migrationBuilder.DropTable(
                name: "Sayfalar");

            migrationBuilder.DropTable(
                name: "SiteAyarlari");

            migrationBuilder.DropTable(
                name: "Slaytlar");
        }
    }
}
