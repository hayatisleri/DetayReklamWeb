using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;

namespace KurumsalWebSitesi.ViewComponents
{
    public class ReferansHaberViewModel
    {
        public List<Referans> Referanslar { get; set; } = new();
        public List<Haber> Haberler { get; set; } = new();
    }

    public class ReferanslarViewComponent : ViewComponent
    {
        private readonly KurumsalDbContext _context;

        public ReferanslarViewComponent(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new ReferansHaberViewModel
            {
                Referanslar = await _context.Referanslar
                    .Where(r => r.AktifMi)
                    .OrderBy(r => r.Sira)
                    .AsNoTracking()
                    .ToListAsync(),

                Haberler = await _context.Haberler
                    .Where(h => h.AktifMi)
                    .OrderByDescending(h => h.Id)
                    .Take(3)
                    .AsNoTracking()
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
