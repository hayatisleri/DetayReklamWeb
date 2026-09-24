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
    public class IletisimMesajiController : Controller
    {
        private readonly KurumsalDbContext _context;

        public IletisimMesajiController(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mesajlar = await _context.IletisimMesajlari
                                         .OrderByDescending(x => x.GonderilmeTarihi)
                                         .ToListAsync();
            return View(mesajlar);
        }

        [HttpGet]
        public async Task<IActionResult> Detay(int? id)
        {
            if (id == null) return NotFound();

            var mesaj = await _context.IletisimMesajlari.FindAsync(id);
            if (mesaj == null) return NotFound();

            if (!mesaj.OkunduMu)
            {
                mesaj.OkunduMu = true;
                _context.Update(mesaj);
                await _context.SaveChangesAsync();
            }

            return View(mesaj);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var mesaj = await _context.IletisimMesajlari.FindAsync(id);
            if (mesaj != null)
            {
                _context.IletisimMesajlari.Remove(mesaj);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}