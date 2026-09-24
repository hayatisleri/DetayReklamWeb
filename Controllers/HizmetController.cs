using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;

namespace KurumsalWebSitesi.Controllers
{
    public class HizmetController : Controller
    {
        private readonly KurumsalDbContext _context;

        public HizmetController(KurumsalDbContext context)
        {
            _context = context;
        }

        // GET: /Hizmet/Detay/5
        public async Task<IActionResult> Detay(int id)
        {
            var hizmet = await _context.Hizmetler.FirstOrDefaultAsync(h => h.Id == id);
            if (hizmet == null)
            {
                return NotFound();
            }

            return View(hizmet);
        }
    }
}