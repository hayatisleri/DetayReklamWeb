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
    public class EkipController : Controller
    {
        private readonly KurumsalDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EkipController(KurumsalDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var ekipListesi = await _context.EkipUyeleri.OrderBy(x => x.Sira).ToListAsync();
            return View(ekipListesi);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(EkipUyesi ekipUyesi, IFormFile? resimDosyasi)
        {
            ModelState.Remove("ResimAdresi");

            ekipUyesi.LinkedInAdresi ??= "";
            ekipUyesi.TwitterAdresi ??= "";
            ekipUyesi.InstagramAdresi ??= "";

            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/ekip");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(stream);
                    }
                    ekipUyesi.ResimAdresi = "/uploads/ekip/" + fileName;
                }

                _context.Add(ekipUyesi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ekipUyesi);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null) return NotFound();

            var ekipUyesi = await _context.EkipUyeleri.FindAsync(id);
            if (ekipUyesi == null) return NotFound();

            return View(ekipUyesi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, EkipUyesi ekipUyesi, IFormFile? resimDosyasi)
        {
            if (id != ekipUyesi.Id) return NotFound();

            ModelState.Remove("ResimAdresi");

            ekipUyesi.LinkedInAdresi ??= "";
            ekipUyesi.TwitterAdresi ??= "";
            ekipUyesi.InstagramAdresi ??= "";

            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    // Eski resmi sil
                    if (!string.IsNullOrEmpty(ekipUyesi.ResimAdresi))
                    {
                        string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, ekipUyesi.ResimAdresi.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                    }

                    // Yeni resmi yükle
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/ekip");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(stream);
                    }
                    ekipUyesi.ResimAdresi = "/uploads/ekip/" + fileName;
                }

                _context.Update(ekipUyesi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ekipUyesi);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var ekipUyesi = await _context.EkipUyeleri.FindAsync(id);
            if (ekipUyesi != null)
            {
                if (!string.IsNullOrEmpty(ekipUyesi.ResimAdresi))
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, ekipUyesi.ResimAdresi.TrimStart('/'));
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }
                _context.EkipUyeleri.Remove(ekipUyesi);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}