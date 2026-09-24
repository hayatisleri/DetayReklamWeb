using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Areas.Admin.Models;
using KurumsalWebSitesi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KurumsalWebSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly KurumsalDbContext _context;

        public HomeController(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index([FromServices] GscApiService gscService)
        {
            var ayar = await _context.SiteAyarlari.FirstOrDefaultAsync();

            var model = new DashboardViewModel
            {
                // KPI Sayıları
                OkunmamisMesajSayisi = await _context.IletisimMesajlari.CountAsync(m => !m.OkunduMu),
                ToplamUrunSayisi = await _context.Urunler.CountAsync(),
                ToplamProjeSayisi = await _context.Projeler.CountAsync(),
                BekleyenBasvuruSayisi = await _context.IsBasvurulari.CountAsync(),

                // Son Hareketler (Müşteriye aktif bir site hissi verir)
                SonGelenMesajlar = await _context.IletisimMesajlari
                                        .OrderByDescending(m => m.Id)
                                        .Take(5)
                                        .ToListAsync(),

                SonEklenenUrunler = await _context.Urunler
                                        .OrderByDescending(u => u.Id)
                                        .Take(5)
                                        .ToListAsync()
            };

            // GSC Verilerini Çek ve Özet İstatistikleri Hesapla
            if (ayar != null && !string.IsNullOrEmpty(ayar.GscPropertyUrl) && !string.IsNullOrEmpty(ayar.GscServiceAccountJsonPath))
            {
                var gscResponse = await gscService.GetSeoVerileriAsync(ayar.GscPropertyUrl, ayar.GscServiceAccountJsonPath);

                if (gscResponse != null && gscResponse.Rows != null && gscResponse.Rows.Any())
                {
                    model.EnIyiSorgular = gscResponse.Rows.Take(8).ToList();

                    // Toplamları ve ortalamaları hesapla
                    model.ToplamTiklama = (long)gscResponse.Rows.Sum(r => r.Clicks ?? 0);
                    model.ToplamGosterim = (long)gscResponse.Rows.Sum(r => r.Impressions ?? 0);

                    if (gscResponse.Rows.Count > 0)
                    {
                        model.OrtalamaPozisyon = gscResponse.Rows.Average(r => r.Position ?? 0);
                        model.OrtalamaCtr = gscResponse.Rows.Average(r => r.Ctr ?? 0) * 100; // Yüzdeye çeviriyoruz
                    }
                }
            }

            return View(model);
        }
    }
}