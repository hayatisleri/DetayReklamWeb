using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;

namespace KurumsalWebSitesi.Controllers
{
    public class ProjeController : Controller
    {
        private readonly KurumsalDbContext _context;

        public ProjeController(KurumsalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projeler = await _context.Projeler
                .Where(p => p.AktifMi)
                .OrderByDescending(p => p.Id)
                .AsNoTracking()
                .ToListAsync();

            return View(projeler);
        }

        // 404 HATASINI ÇÖZEN DETAY METODU
        [HttpGet]
        public async Task<IActionResult> Detay(int id)
        {
            var proje = await _context.Projeler
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.AktifMi);

            if (proje == null)
            {
                return NotFound();
            }

            return View(proje);
        }
    }
}