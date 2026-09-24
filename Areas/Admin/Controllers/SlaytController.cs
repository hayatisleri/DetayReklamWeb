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
    public class SlaytController : Controller
    {
        private readonly KurumsalDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlaytController(KurumsalDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // 1. Listeleme
        public async Task<IActionResult> Index()
        {
            var slaytlar = await _context.Slaytlar.OrderBy(x => x.Sira).ToListAsync();
            return View(slaytlar);
        }

        // 2. Ekleme (Get)
        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        // 3. Ekleme (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Slayt slayt, IFormFile? resimDosyasi)
        {
            // Formdan gelen dosya kontrolü
            var yuklenenDosya = resimDosyasi ?? (Request.Form.Files.Count > 0 ? Request.Form.Files[0] : null);

            // Validasyona takılabilecek alanları temizle
            ModelState.Remove("ResimAdresi");
            ModelState.Remove("resimDosyasi");
            ModelState.Remove("LinkAdresi");

            if (yuklenenDosya == null || yuklenenDosya.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen bir slayt görseli seçin.");
                return View(slayt);
            }

            try
            {
                // uploads/slaytlar klasörünü oluştur
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "slaytlar");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Benzersiz dosya adı oluştur ve kaydet
                string fileExtension = Path.GetExtension(yuklenenDosya.FileName);
                string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await yuklenenDosya.CopyToAsync(fileStream);
                }

                slayt.ResimAdresi = "/uploads/slaytlar/" + uniqueFileName;

                // Boş bırakılan alanlara varsayılan değer ata
                if (string.IsNullOrWhiteSpace(slayt.Baslik)) slayt.Baslik = "Detay Reklam";
                if (string.IsNullOrWhiteSpace(slayt.AltBaslik)) slayt.AltBaslik = "Tabela & Reklam Çözümleri";

                _context.Slaytlar.Add(slayt);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Kayıt sırasında hata oluştu: " + ex.Message);
                return View(slayt);
            }
        }

        // 4. Düzenleme (Get)
        [HttpGet]
        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null) return NotFound();

            var slayt = await _context.Slaytlar.FindAsync(id);
            if (slayt == null) return NotFound();

            return View(slayt);
        }

        // 5. Düzenleme (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Slayt slayt, IFormFile? resimDosyasi)
        {
            if (id != slayt.Id) return NotFound();

            var yuklenenDosya = resimDosyasi ?? (Request.Form.Files.Count > 0 ? Request.Form.Files[0] : null);

            ModelState.Remove("ResimAdresi");
            ModelState.Remove("resimDosyasi");
            ModelState.Remove("LinkAdresi");

            try
            {
                if (yuklenenDosya != null && yuklenenDosya.Length > 0)
                {
                    // Eski resmi diskten sil
                    if (!string.IsNullOrEmpty(slayt.ResimAdresi))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, slayt.ResimAdresi.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Yeni resmi yükle
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "slaytlar");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileExtension = Path.GetExtension(yuklenenDosya.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await yuklenenDosya.CopyToAsync(fileStream);
                    }

                    slayt.ResimAdresi = "/uploads/slaytlar/" + uniqueFileName;
                }

                _context.Update(slayt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Güncelleme sırasında hata oluştu: " + ex.Message);
                return View(slayt);
            }
        }

        // 6. Silme
        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var slayt = await _context.Slaytlar.FindAsync(id);
            if (slayt != null)
            {
                if (!string.IsNullOrEmpty(slayt.ResimAdresi))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, slayt.ResimAdresi.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Slaytlar.Remove(slayt);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}