using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;

namespace KurumsalWebSitesi.Controllers
{
    public class UrunController : Controller
    {
        private readonly KurumsalDbContext _context;

        public UrunController(KurumsalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var urunler = await _context.Urunler
                .OrderByDescending(u => u.Id)
                .AsNoTracking()
                .ToListAsync();

            return View(urunler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SiparisGonder(int UrunId, string UrunAdi, string AdSoyad, string Telefon, string? Eposta, string? Mesaj)
        {
            try
            {
                var yeniTalep = new IletisimMesaji
                {
                    AdSoyad = AdSoyad,
                    Telefon = Telefon,
                    Eposta = !string.IsNullOrWhiteSpace(Eposta) ? Eposta : "belirtilmedi@iletisim.com",
                    Konu = $"Sipariş / Teklif: {UrunAdi}",
                    Mesaj = $"Ürün: {UrunAdi} (ID: {UrunId})\nİletişim Tel: {Telefon}\nNot: {Mesaj ?? "Not girilmedi."}",
                    OkunduMu = false
                };

                _context.IletisimMesajlari.Add(yeniTalep);
                await _context.SaveChangesAsync();

                TempData["Mesaj"] = $"'{UrunAdi}' için sipariş talebiniz başarıyla iletildi. En kısa sürede sizinle iletişime geçilecektir.";
            }
            catch
            {
                TempData["Mesaj"] = $"'{UrunAdi}' için talebiniz alınmıştır.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}