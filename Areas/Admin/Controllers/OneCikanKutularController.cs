using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;

namespace KurumsalWebSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OneCikanKutularController : Controller
    {
        private readonly KurumsalDbContext _context;

        public OneCikanKutularController(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var liste = await _context.OneCikanKutular.OrderBy(x => x.Sira).ToListAsync();
            return View(liste);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OneCikanKutu kutu)
        {
            if (ModelState.IsValid)
            {
                _context.OneCikanKutular.Add(kutu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kutu);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var kutu = await _context.OneCikanKutular.FindAsync(id);
            if (kutu != null)
            {
                _context.OneCikanKutular.Remove(kutu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}