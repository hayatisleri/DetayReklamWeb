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
            // Resim adresi formdan gelmediği için validasyonu geçmek adına modelden çıkarıyoruz
            ModelState.Remove("ResimAdresi");

            // Boş gelebilecek alanları güvenli hale getiriyoruz
            haber.Ozet ??= "";
            haber.Icerik ??= "";
            haber.ResimAdresi ??= "";

            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "haberler");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + resimDosyasi.FileName;
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

            ModelState.Remove("ResimAdresi");

            haber.Ozet ??= "";
            haber.Icerik ??= "";
            haber.ResimAdresi ??= "";

            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    if (!string.IsNullOrEmpty(haber.ResimAdresi))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, haber.ResimAdresi.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "haberler");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + resimDosyasi.FileName;
                    using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(fileStream);
                    }
                    haber.ResimAdresi = "/uploads/haberler/" + uniqueFileName;
                }

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
    }
}