using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KurumsalWebSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MenuController : Controller
    {
        private readonly KurumsalDbContext _context;

        public MenuController(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var menuler = await _context.Menuler.OrderBy(m => m.UstMenuId).ThenBy(m => m.Sira).ToListAsync();
            return View(menuler);
        }

        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            ViewBag.AnaMenuler = await _context.Menuler.Where(m => m.UstMenuId == null).OrderBy(m => m.Sira).ToListAsync();

            // Müşteriye sunacağımız hazır sayfalar listesi
            ViewBag.SayfaListesi = new Dictionary<string, string>
    {
        { "Anasayfa", "/" },
        { "Hakkımızda", "/Sayfa/Hakkimizda" },
        { "Ürünler", "/Urun/Index" },
        { "Projeler", "/Proje/Index" },
        { "Hizmetler", "/Hizmet/Index" },
        { "Haberler", "/Haber/Index" },
        { "Galeri", "/Galeri/Index" },
        { "İletişim", "/Iletisim/Index" },
        { "İş Başvurusu", "/IsBasvurusu/Ekle" }
    };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AnaMenuler = await _context.Menuler.Where(m => m.UstMenuId == null).OrderBy(m => m.Sira).ToListAsync();
            return View(menu);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null) return NotFound();

            var menu = await _context.Menuler.FindAsync(id);
            if (menu == null) return NotFound();

            ViewBag.AnaMenuler = await _context.Menuler.Where(m => m.UstMenuId == null && m.Id != id).OrderBy(m => m.Sira).ToListAsync();
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Menu menu)
        {
            if (id != menu.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AnaMenuler = await _context.Menuler.Where(m => m.UstMenuId == null && m.Id != id).OrderBy(m => m.Sira).ToListAsync();
            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var menu = await _context.Menuler.FindAsync(id);
            if (menu != null)
            {
                // Alt menüleri var mı kontrol et
                var altMenuler = await _context.Menuler.Where(m => m.UstMenuId == id).ToListAsync();
                if (altMenuler.Any())
                {
                    _context.Menuler.RemoveRange(altMenuler); // Önce alt menüleri sil
                }

                _context.Menuler.Remove(menu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}