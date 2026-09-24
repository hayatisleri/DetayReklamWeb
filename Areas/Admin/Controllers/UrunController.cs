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
    public class UrunController : Controller
    {
        private readonly KurumsalDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UrunController(KurumsalDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index() => View(await _context.Urunler.ToListAsync());

        [HttpGet]
        public IActionResult Ekle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Urun urun, IFormFile? resimDosyasi)
        {
            ModelState.Remove("ResimAdresi");
            urun.Aciklama ??= "";
            urun.TeknikDetaylar ??= "";

            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/urunler");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                        await resimDosyasi.CopyToAsync(stream);
                    urun.ResimAdresi = "/uploads/urunler/" + fileName;
                }
                _context.Add(urun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(urun);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null) return NotFound();
            var urun = await _context.Urunler.FindAsync(id);
            return urun == null ? NotFound() : View(urun);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Urun urun, IFormFile? resimDosyasi)
        {
            if (id != urun.Id) return NotFound();
            ModelState.Remove("ResimAdresi");

            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    if (!string.IsNullOrEmpty(urun.ResimAdresi))
                    {
                        string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, urun.ResimAdresi.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    using (var stream = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, "uploads/urunler", fileName), FileMode.Create))
                        await resimDosyasi.CopyToAsync(stream);
                    urun.ResimAdresi = "/uploads/urunler/" + fileName;
                }
                _context.Update(urun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(urun);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var urun = await _context.Urunler.FindAsync(id);
            if (urun != null)
            {
                if (!string.IsNullOrEmpty(urun.ResimAdresi))
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, urun.ResimAdresi.TrimStart('/'));
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }
                _context.Urunler.Remove(urun);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}