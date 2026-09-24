using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;

namespace KurumsalWebSitesi.Controllers
{
    public class IletisimController : Controller
    {
        private readonly KurumsalDbContext _context;

        public IletisimController(KurumsalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var siteAyari = await _context.SiteAyarlari.FirstOrDefaultAsync();
            return View(siteAyari);
        }
    }
}