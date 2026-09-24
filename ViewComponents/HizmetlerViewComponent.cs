using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;

namespace KurumsalWebSitesi.ViewComponents
{
    public class HizmetlerViewComponent : ViewComponent
    {
        private readonly KurumsalDbContext _context;

        public HizmetlerViewComponent(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hizmetler = await _context.Hizmetler
                .Where(h => h.AktifMi)
                .OrderBy(h => h.Sira)
                .AsNoTracking()
                .ToListAsync();

            return View(hizmetler);
        }
    }
}