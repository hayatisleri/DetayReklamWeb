using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KurumsalWebSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SayfaController : Controller
    {
        private readonly KurumsalDbContext _context;

        public SayfaController(KurumsalDbContext context)
        {
            _context = context;
        }

        // 1. Listeleme
        public async Task<IActionResult> Index()
        {
            var sayfalar = await _context.Sayfalar.OrderByDescending(x => x.OlusturulmaTarihi).ToListAsync();
            return View(sayfalar);
        }

        // 2. Yeni Sayfa Ekleme (Get)
        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        // 3. Yeni Sayfa Ekleme (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Sayfa sayfa)
        {
            // SEO URL kontrolü
            if (string.IsNullOrWhiteSpace(sayfa.SeoUrl))
            {
                sayfa.SeoUrl = !string.IsNullOrWhiteSpace(sayfa.Baslik) ? sayfa.Baslik : "hakkimizda";
            }

            sayfa.SeoUrl = sayfa.SeoUrl.Trim().ToLower()
                .Replace(" ", "-").Replace("ı", "i").Replace("ğ", "g")
                .Replace("ü", "u").Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");

            // SQL NULL Hatalarını Önleyen Varsayılan Değerler:
            if (string.IsNullOrWhiteSpace(sayfa.SeoBasligi))
            {
                sayfa.SeoBasligi = sayfa.Baslik ?? "Hakkımızda";
            }

            if (string.IsNullOrWhiteSpace(sayfa.SeoAciklamasi))
            {
                sayfa.SeoAciklamasi = sayfa.Baslik ?? "Hakkımızda Kurumsal Sayfası";
            }

            if (string.IsNullOrWhiteSpace(sayfa.Icerik))
            {
                sayfa.Icerik = "";
            }

            sayfa.OlusturulmaTarihi = DateTime.Now;

            _context.Add(sayfa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 4. Düzenleme (Get)
        [HttpGet]
        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null) return NotFound();

            var sayfa = await _context.Sayfalar.FindAsync(id);
            if (sayfa == null) return NotFound();

            return View(sayfa);
        }

        // 5. Düzenleme (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Sayfa sayfa)
        {
            if (id != sayfa.Id) return NotFound();

            var mevcut = await _context.Sayfalar.FindAsync(id);
            if (mevcut == null) return NotFound();

            mevcut.Baslik = sayfa.Baslik;
            mevcut.Icerik = sayfa.Icerik ?? "";
            mevcut.SeoBasligi = string.IsNullOrWhiteSpace(sayfa.SeoBasligi) ? (sayfa.Baslik ?? "Hakkımızda") : sayfa.SeoBasligi;
            mevcut.SeoAciklamasi = string.IsNullOrWhiteSpace(sayfa.SeoAciklamasi) ? (sayfa.Baslik ?? "Hakkımızda Kurumsal Sayfası") : sayfa.SeoAciklamasi;
            mevcut.AktifMi = sayfa.AktifMi;

            if (!string.IsNullOrWhiteSpace(sayfa.SeoUrl))
            {
                mevcut.SeoUrl = sayfa.SeoUrl.Trim().ToLower()
                    .Replace(" ", "-").Replace("ı", "i").Replace("ğ", "g")
                    .Replace("ü", "u").Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 6. Silme
        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var sayfa = await _context.Sayfalar.FindAsync(id);
            if (sayfa != null)
            {
                _context.Sayfalar.Remove(sayfa);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}