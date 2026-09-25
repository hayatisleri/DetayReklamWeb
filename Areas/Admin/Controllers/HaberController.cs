using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KurumsalWebSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HaberController : Controller
    {
        private readonly KurumsalDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HaberController(KurumsalDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var haberler = await _context.Haberler.OrderByDescending(x => x.OlusturulmaTarihi).ToListAsync();
            return View(haberler);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Haber haber, IFormFile? resimDosyasi)
        {
            // Validasyona takılan tüm zorunlu alanların kontrolünü kaldırıyoruz
            ModelState.Remove("ResimAdresi");
            ModelState.Remove("SeoUrl");
            ModelState.Remove("Ozet");
            ModelState.Remove("Icerik");

            // Null olabilecek alanlara boş string atayarak veritabanı kısıtlamalarını garantiye alıyoruz
            haber.Ozet = string.IsNullOrWhiteSpace(haber.Ozet) ? "" : haber.Ozet;
            haber.Icerik = string.IsNullOrWhiteSpace(haber.Icerik) ? "" : haber.Icerik;
            haber.ResimAdresi ??= "";

            // SeoUrl boş geldiyse başlıktan otomatik üret
            if (string.IsNullOrWhiteSpace(haber.SeoUrl))
            {
                haber.SeoUrl = SeoUrlUret(haber.Baslik);
            }
            else
            {
                haber.SeoUrl = SeoUrlUret(haber.SeoUrl);
            }

            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "haberler");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(fileStream);
                    }
                    haber.ResimAdresi = "/uploads/haberler/" + uniqueFileName;
                }

                haber.OlusturulmaTarihi = DateTime.Now;
                _context.Add(haber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(haber);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null) return NotFound();
            var haber = await _context.Haberler.FindAsync(id);
            if (haber == null) return NotFound();
            return View(haber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Haber haber, IFormFile? resimDosyasi)
        {
            if (id != haber.Id) return NotFound();

            // Düzenleme sırasında da doğrulama takılmalarını engelliyoruz
            ModelState.Remove("ResimAdresi");
            ModelState.Remove("SeoUrl");
            ModelState.Remove("Ozet");
            ModelState.Remove("Icerik");

            haber.Ozet = string.IsNullOrWhiteSpace(haber.Ozet) ? "" : haber.Ozet;
            haber.Icerik = string.IsNullOrWhiteSpace(haber.Icerik) ? "" : haber.Icerik;

            if (string.IsNullOrWhiteSpace(haber.SeoUrl))
            {
                haber.SeoUrl = SeoUrlUret(haber.Baslik);
            }
            else
            {
                haber.SeoUrl = SeoUrlUret(haber.SeoUrl);
            }

            if (ModelState.IsValid)
            {
                var mevcutHaber = await _context.Haberler.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (mevcutHaber == null) return NotFound();

                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    if (!string.IsNullOrEmpty(mevcutHaber.ResimAdresi))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, mevcutHaber.ResimAdresi.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "haberler");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(fileStream);
                    }
                    haber.ResimAdresi = "/uploads/haberler/" + uniqueFileName;
                }
                else
                {
                    haber.ResimAdresi = mevcutHaber.ResimAdresi;
                }

                haber.OlusturulmaTarihi = mevcutHaber.OlusturulmaTarihi;
                _context.Update(haber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(haber);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var haber = await _context.Haberler.FindAsync(id);
            if (haber != null)
            {
                if (!string.IsNullOrEmpty(haber.ResimAdresi))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, haber.ResimAdresi.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                }
                _context.Haberler.Remove(haber);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private static string SeoUrlUret(string? metin)
        {
            if (string.IsNullOrWhiteSpace(metin)) return Guid.NewGuid().ToString().Substring(0, 8);

            string str = metin.ToLowerInvariant();
            str = str.Replace("ı", "i")
                     .Replace("ğ", "g")
                     .Replace("ü", "u")
                     .Replace("ş", "s")
                     .Replace("ö", "o")
                     .Replace("ç", "c");

            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = str.Substring(0, str.Length <= 100 ? str.Length : 100).Trim();
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }
    }
}