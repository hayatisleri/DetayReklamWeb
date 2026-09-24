using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;

namespace KurumsalWebSitesi.Controllers
{
    public class GaleriController : Controller
    {
        private readonly KurumsalDbContext _context;

        public GaleriController(KurumsalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var resimler = await _context.Galeriler
                .OrderByDescending(g => g.Id)
                .AsNoTracking()
                .ToListAsync();

            return View(resimler);
        }
    }
}