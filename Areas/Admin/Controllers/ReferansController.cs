using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;

namespace KurumsalWebSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReferansController : Controller
    {
        private readonly KurumsalDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ReferansController(KurumsalDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var referanslar = await _context.Referanslar
                .OrderBy(r => r.Sira)
                .AsNoTracking()
                .ToListAsync();
            return View(referanslar);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Referans referans, IFormFile? logoDosyasi)
        {
            if (logoDosyasi != null && logoDosyasi.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "referans");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(logoDosyasi.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await logoDosyasi.CopyToAsync(stream);
                }

                referans.LogoAdresi = "/uploads/referans/" + uniqueFileName;
            }

            _context.Referanslar.Add(referans);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var referans = await _context.Referanslar.FindAsync(id);
            if (referans != null)
            {
                if (!string.IsNullOrEmpty(referans.LogoAdresi))
                {
                    var fullPath = Path.Combine(_env.WebRootPath, referans.LogoAdresi.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                _context.Referanslar.Remove(referans);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
