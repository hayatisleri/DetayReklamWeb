using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;

namespace KurumsalWebSitesi.Controllers
{
    public class ReferansController : Controller
    {
        private readonly KurumsalDbContext _context;

        public ReferansController(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var referanslar = await _context.Referanslar
                .Where(r => r.AktifMi)
                .OrderBy(r => r.Sira)
                .AsNoTracking()
                .ToListAsync();

            return View(referanslar);
        }
    }
}
