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
    public class GaleriController : Controller
    {
        private readonly KurumsalDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GaleriController(KurumsalDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Galeriler.OrderBy(x => x.Sira).ToListAsync());
        }

        [HttpGet]
        public IActionResult Ekle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Galeri galeri, IFormFile? resimDosyasi)
        {
            ModelState.Remove("ResimAdresi");
            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/galeri");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                        await resimDosyasi.CopyToAsync(stream);
                    galeri.ResimAdresi = "/uploads/galeri/" + fileName;
                }
                _context.Add(galeri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(galeri);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var galeri = await _context.Galeriler.FindAsync(id);
            if (galeri == null) return NotFound();
            return View(galeri); // Veriyi View'a gönderiyoruz
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Galeri galeri, IFormFile? resimDosyasi)
        {
            if (id != galeri.Id) return NotFound();
            ModelState.Remove("ResimAdresi");

            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    if (!string.IsNullOrEmpty(galeri.ResimAdresi))
                    {
                        string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, galeri.ResimAdresi.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    using (var stream = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, "uploads/galeri", fileName), FileMode.Create))
                        await resimDosyasi.CopyToAsync(stream);
                    galeri.ResimAdresi = "/uploads/galeri/" + fileName;
                }
                _context.Update(galeri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(galeri);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var galeri = await _context.Galeriler.FindAsync(id);
            if (galeri != null)
            {
                if (!string.IsNullOrEmpty(galeri.ResimAdresi))
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, galeri.ResimAdresi.TrimStart('/'));
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }
                _context.Galeriler.Remove(galeri);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}