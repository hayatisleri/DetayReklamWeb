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
    public class HizmetController : Controller
    {
        private readonly KurumsalDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // IWebHostEnvironment arayüzünü Dependency Injection ile içeri aktarıyoruz
        public HizmetController(KurumsalDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // 1. Listeleme
        public async Task<IActionResult> Index()
        {
            // Hizmetleri Sıra numarasına göre listeliyoruz
            var hizmetler = await _context.Hizmetler.OrderBy(x => x.Sira).ToListAsync();
            return View(hizmetler);
        }

        // 2. Ekleme (Get - Formu Getir)
        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        // 3. Ekleme (Post - Veriyi ve Dosyayı Kaydet)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Hizmet hizmet, IFormFile? resimDosyasi)
        {
            // ResimAdresi alanı formdan gelmediği için (biz arka planda atayacağız) doğrulamadan çıkarıyoruz
            ModelState.Remove("ResimAdresi");

            // Null hatası almamak için boş gelebilecek metin alanlarına varsayılan boş string atıyoruz
            hizmet.KisaAciklama ??= "";
            hizmet.Icerik ??= "";
            hizmet.ResimAdresi ??= "";

            if (ModelState.IsValid)
            {
                // Resim Yükleme İşlemi
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "hizmetler");

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Benzersiz dosya adı oluşturma (Aynı isimli dosyaların çakışmasını önler)
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + resimDosyasi.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Dosyayı sunucuya kopyala
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(fileStream);
                    }

                    // Veritabanına kaydedilecek URL adresini belirle
                    hizmet.ResimAdresi = "/uploads/hizmetler/" + uniqueFileName;
                }

                hizmet.OlusturulmaTarihi = DateTime.Now;
                _context.Add(hizmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hizmet);
        }

        // 4. Düzenleme (Get)
        [HttpGet]
        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null) return NotFound();

            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet == null) return NotFound();

            return View(hizmet);
        }

        // 5. Düzenleme (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Hizmet hizmet, IFormFile? resimDosyasi)
        {
            if (id != hizmet.Id) return NotFound();

            // ResimAdresi alanını doğrulamadan çıkarıyoruz
            ModelState.Remove("ResimAdresi");

            // Null hatası almamak için boş gelebilecek metin alanlarına varsayılan boş string atıyoruz
            hizmet.KisaAciklama ??= "";
            hizmet.Icerik ??= "";
            hizmet.ResimAdresi ??= "";

            if (ModelState.IsValid)
            {
                // Yeni resim yüklendiyse eski resmi fiziksel olarak sil ve yenisini kaydet
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    // Eski resmi bul ve sil
                    if (!string.IsNullOrEmpty(hizmet.ResimAdresi))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, hizmet.ResimAdresi.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Yeni resmi yükle
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "hizmetler");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + resimDosyasi.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(fileStream);
                    }

                    hizmet.ResimAdresi = "/uploads/hizmetler/" + uniqueFileName;
                }

                _context.Update(hizmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hizmet);
        }

        // 6. Silme
        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet != null)
            {
                // Kaydı veritabanından silerken, dosyayı da sunucudan fiziksel olarak temizliyoruz
                if (!string.IsNullOrEmpty(hizmet.ResimAdresi))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, hizmet.ResimAdresi.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Hizmetler.Remove(hizmet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}