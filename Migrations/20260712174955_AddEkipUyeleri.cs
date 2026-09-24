using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalWebSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AddEkipUyeleri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EkipUyeleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Unvan = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ResimAdresi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedInAdresi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TwitterAdresi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InstagramAdresi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EkipUyeleri", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EkipUyeleri");
        }
    }
}
