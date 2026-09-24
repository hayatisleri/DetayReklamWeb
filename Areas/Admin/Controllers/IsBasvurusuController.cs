using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

[Area("Admin")]
[Authorize]
public class IsBasvurusuController : Controller
{
    private readonly KurumsalDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public IsBasvurusuController(KurumsalDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index() =>
        View(await _context.IsBasvurulari.OrderByDescending(x => x.BasvuruTarihi).ToListAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(IsBasvurusu basvuru, IFormFile cvDosyasi)
    {
        if (ModelState.IsValid)
        {
            if (cvDosyasi != null && cvDosyasi.Length > 0)
            {
                // 1. Klasör yolunu belirle
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/ik");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                // 2. Benzersiz isim oluştur (Dosya uzantısını koru)
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(cvDosyasi.FileName);
                string filePath = Path.Combine(folder, fileName);

                // 3. Dosyayı kaydet
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await cvDosyasi.CopyToAsync(stream);
                }

                // 4. Veritabanına yolu kaydet
                basvuru.CvDosyaYolu = "/uploads/ik/" + fileName;
            }

            _context.Add(basvuru);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home"); // Başvuru sonrası yönlendirme
        }
        return View(basvuru);
    }

    [HttpPost]
    public async Task<IActionResult> Sil(int id)
    {
        var basvuru = await _context.IsBasvurulari.FindAsync(id);
        if (basvuru != null)
        {
            // Eğer CV dosyası sunucuda kayıtlıysa onu da silelim
            if (!string.IsNullOrEmpty(basvuru.CvDosyaYolu))
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, basvuru.CvDosyaYolu.TrimStart('/'));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            _context.IsBasvurulari.Remove(basvuru);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}