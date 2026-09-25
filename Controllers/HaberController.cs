using KurumsalWebSitesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KurumsalWebSitesi.Controllers
{
    public class HaberController : Controller
    {
        private readonly KurumsalDbContext _context;

        public HaberController(KurumsalDbContext context)
        {
            _context = context;
        }

        // /Haber adresine tıklandığında çalışan liste sayfası
        public async Task<IActionResult> Index()
        {
            var haberler = await _context.Haberler
                .Where(x => x.AktifMi)
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .ToListAsync();

            return View(haberler);
        }

        // Habere tıklandığında detayını açan sayfa
        public async Task<IActionResult> Detay(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var haber = await _context.Haberler
                .FirstOrDefaultAsync(x => x.SeoUrl == id || x.Id.ToString() == id);

            if (haber == null || !haber.AktifMi) return NotFound();

            return View(haber);
        }
    }
}