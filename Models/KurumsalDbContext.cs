using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models.Entities;

namespace KurumsalWebSitesi.Models
{
    public class KurumsalDbContext : DbContext
    {
        public KurumsalDbContext(DbContextOptions<KurumsalDbContext> options) : base(options)
        {
        }

        public DbSet<SiteAyari> SiteAyarlari { get; set; }
        public DbSet<Sayfa> Sayfalar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Slayt> Slaytlar { get; set; }
        public DbSet<IletisimMesaji> IletisimMesajlari { get; set; }
        public DbSet<Haber> Haberler { get; set; }
        public DbSet<Galeri> Galeriler { get; set; }
        public DbSet<Referans> Referanslar { get; set; }
        public DbSet<Proje> Projeler { get; set; }
        public DbSet<IsBasvurusu> IsBasvurulari { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<KurumsalWebSitesi.Models.Entities.MusteriYorumu> MusteriYorumlari { get; set; }
        public DbSet<EkipUyesi> EkipUyeleri { get; set; }
        public DbSet<Menu> Menuler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<KurumsalWebSitesi.Models.Entities.OneCikanKutu> OneCikanKutular { get; set; }
    }
}