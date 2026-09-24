using KurumsalWebSitesi.Models.Entities;
using System.Collections.Generic;

namespace KurumsalWebSitesi.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        // 1. Temel İstatistikler (KPI)
        public int OkunmamisMesajSayisi { get; set; }
        public int ToplamUrunSayisi { get; set; }
        public int ToplamProjeSayisi { get; set; }
        public int BekleyenBasvuruSayisi { get; set; }

        // 2. Operasyonel Veriler (Sistemin Yaşadığını Gösterenler)
        public List<IletisimMesaji> SonGelenMesajlar { get; set; }
        public List<Urun> SonEklenenUrunler { get; set; }

        // 3. Google Search Console (SEO) Özet Verileri
        public long ToplamTiklama { get; set; }
        public long ToplamGosterim { get; set; }
        public double OrtalamaPozisyon { get; set; }
        public double OrtalamaCtr { get; set; }
        public List<Google.Apis.SearchConsole.v1.Data.ApiDataRow> EnIyiSorgular { get; set; }
    }
}