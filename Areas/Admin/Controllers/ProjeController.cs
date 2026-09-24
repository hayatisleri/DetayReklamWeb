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
    public class ProjeController : Controller
    {
        private readonly KurumsalDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjeController(KurumsalDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index() => View(await _context.Projeler.OrderByDescending(x => x.Id).ToListAsync());

        [HttpGet]
        public IActionResult Ekle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Proje proje, IFormFile? kapakResmi)
        {
            // Resim ve içerik doğrulamalarını ModelState'ten temizle
            ModelState.Remove("KapakResmi");
            ModelState.Remove("kapakResmi");
            ModelState.Remove("Icerik");

            proje.Icerik ??= "";
            proje.KapakResmi ??= "";

            if (ModelState.IsValid)
            {
                if (kapakResmi != null && kapakResmi.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/projeler");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(kapakResmi.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await kapakResmi.CopyToAsync(stream);
                    }
                    proje.KapakResmi = "/uploads/projeler/" + fileName;
                }

                _context.Projeler.Add(proje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(proje);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null) return NotFound();
            var proje = await _context.Projeler.FindAsync(id);
            return proje == null ? NotFound() : View(proje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Proje proje, IFormFile? kapakResmi)
        {
            if (id != proje.Id) return NotFound();

            ModelState.Remove("KapakResmi");
            ModelState.Remove("kapakResmi");
            ModelState.Remove("Icerik");

            proje.Icerik ??= "";

            if (ModelState.IsValid)
            {
                if (kapakResmi != null && kapakResmi.Length > 0)
                {
                    if (!string.IsNullOrEmpty(proje.KapakResmi))
                    {
                        string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, proje.KapakResmi.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                    }
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/projeler");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(kapakResmi.FileName);
                    using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                    {
                        await kapakResmi.CopyToAsync(stream);
                    }
                    proje.KapakResmi = "/uploads/projeler/" + fileName;
                }
                else
                {
                    // Yeni görsel seçilmediyse eski görsel yolunu koru
                    var mevcutProje = await _context.Projeler.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    if (mevcutProje != null)
                    {
                        proje.KapakResmi = mevcutProje.KapakResmi;
                    }
                }

                _context.Update(proje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proje);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var proje = await _context.Projeler.FindAsync(id);
            if (proje != null)
            {
                if (!string.IsNullOrEmpty(proje.KapakResmi))
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, proje.KapakResmi.TrimStart('/'));
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }
                _context.Projeler.Remove(proje);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}