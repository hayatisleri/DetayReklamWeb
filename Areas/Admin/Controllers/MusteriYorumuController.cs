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
    public class MusteriYorumuController : Controller
    {
        private readonly KurumsalDbContext _context;

        public MusteriYorumuController(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var yorumlar = await _context.MusteriYorumlari.OrderBy(y => y.Sira).ToListAsync();
            return View(yorumlar);
        }

        [HttpGet]
        public IActionResult Ekle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(MusteriYorumu model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.MusteriYorumlari.FindAsync(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, MusteriYorumu model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var item = await _context.MusteriYorumlari.FindAsync(id);
            if (item != null)
            {
                _context.MusteriYorumlari.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}