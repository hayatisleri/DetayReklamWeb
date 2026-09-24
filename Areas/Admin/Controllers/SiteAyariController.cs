using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KurumsalWebSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SiteAyariController : Controller
    {
        private readonly KurumsalDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SiteAyariController(KurumsalDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ayar = await _context.SiteAyarlari.FirstOrDefaultAsync();

            if (ayar == null)
            {
                ayar = new SiteAyari
                {
                    SiteBasligi = "Detay Reklam",
                    SiteAciklamasi = "",
                    LogoAdresi = "",
                    FaviconAdresi = "",
                    TelefonNumarasi = "",
                    EpostaAdresi = "",
                    Adres = "",
                    FacebookAdresi = "",
                    InstagramAdresi = "",
                    LinkedInAdresi = "",
                    GoogleAnalyticsId = "",
                    GscPropertyUrl = "",
                    GscServiceAccountJsonPath = "",
                    GuncellenmeTarihi = DateTime.Now
                };
                _context.SiteAyarlari.Add(ayar);
                await _context.SaveChangesAsync();
            }

            return View(ayar);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var ayar = await _context.SiteAyarlari.FindAsync(id);
            if (ayar == null) return NotFound();

            return View(ayar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(SiteAyari ayar, IFormFile? logoDosyasi, IFormFile? faviconDosyasi)
        {
            // Veritabanındaki mevcut kaydı çekiyoruz
            var mevcutAyar = await _context.SiteAyarlari.FindAsync(ayar.Id);
            if (mevcutAyar == null)
            {
                return NotFound();
            }

            // Metin alanlarını güvenle güncelliyoruz
            mevcutAyar.SiteBasligi = string.IsNullOrWhiteSpace(ayar.SiteBasligi) ? "Detay Reklam" : ayar.SiteBasligi;
            mevcutAyar.SiteAciklamasi = ayar.SiteAciklamasi ?? "";
            mevcutAyar.TelefonNumarasi = ayar.TelefonNumarasi ?? "";
            mevcutAyar.EpostaAdresi = ayar.EpostaAdresi ?? "";
            mevcutAyar.Adres = ayar.Adres ?? "";
            mevcutAyar.FacebookAdresi = ayar.FacebookAdresi ?? "";
            mevcutAyar.InstagramAdresi = ayar.InstagramAdresi ?? "";
            mevcutAyar.LinkedInAdresi = ayar.LinkedInAdresi ?? "";
            mevcutAyar.GoogleAnalyticsId = ayar.GoogleAnalyticsId ?? "";
            mevcutAyar.GscPropertyUrl = ayar.GscPropertyUrl ?? "";
            mevcutAyar.GscServiceAccountJsonPath = ayar.GscServiceAccountJsonPath ?? "";

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "site");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Yeni logo yüklendiyse eskisini silip yenisini kaydediyoruz
            if (logoDosyasi != null && logoDosyasi.Length > 0)
            {
                if (!string.IsNullOrEmpty(mevcutAyar.LogoAdresi))
                {
                    string oldLogoPath = Path.Combine(_webHostEnvironment.WebRootPath, mevcutAyar.LogoAdresi.TrimStart('/'));
                    if (System.IO.File.Exists(oldLogoPath))
                    {
                        System.IO.File.Delete(oldLogoPath);
                    }
                }

                string uniqueLogoName = "logo_" + Guid.NewGuid().ToString() + Path.GetExtension(logoDosyasi.FileName);
                string logoPath = Path.Combine(uploadsFolder, uniqueLogoName);

                using (var fileStream = new FileStream(logoPath, FileMode.Create))
                {
                    await logoDosyasi.CopyToAsync(fileStream);
                }
                mevcutAyar.LogoAdresi = "/uploads/site/" + uniqueLogoName;
            }

            // Yeni favicon yüklendiyse eskisini silip yenisini kaydediyoruz
            if (faviconDosyasi != null && faviconDosyasi.Length > 0)
            {
                if (!string.IsNullOrEmpty(mevcutAyar.FaviconAdresi))
                {
                    string oldFaviconPath = Path.Combine(_webHostEnvironment.WebRootPath, mevcutAyar.FaviconAdresi.TrimStart('/'));
                    if (System.IO.File.Exists(oldFaviconPath))
                    {
                        System.IO.File.Delete(oldFaviconPath);
                    }
                }

                string uniqueFaviconName = "favicon_" + Guid.NewGuid().ToString() + Path.GetExtension(faviconDosyasi.FileName);
                string faviconPath = Path.Combine(uploadsFolder, uniqueFaviconName);

                using (var fileStream = new FileStream(faviconPath, FileMode.Create))
                {
                    await faviconDosyasi.CopyToAsync(fileStream);
                }
                mevcutAyar.FaviconAdresi = "/uploads/site/" + uniqueFaviconName;
            }

            mevcutAyar.GuncellenmeTarihi = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = "Site ayarları başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifreGuncelle(string yeniKullaniciAdi, string yeniSifre)
        {
            var mevcutKullaniciAdi = User.Identity.Name;
            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.KullaniciAdi == mevcutKullaniciAdi);

            if (kullanici != null)
            {
                kullanici.KullaniciAdi = yeniKullaniciAdi;

                if (!string.IsNullOrEmpty(yeniSifre))
                {
                    kullanici.SifreHash = BCrypt.Net.BCrypt.HashPassword(yeniSifre);
                }

                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Hesap bilgileriniz başarıyla güncellendi. Lütfen tekrar giriş yapın.";

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Giris", new { area = "Admin" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}