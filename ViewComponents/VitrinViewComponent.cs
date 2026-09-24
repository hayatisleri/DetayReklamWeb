using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;

namespace KurumsalWebSitesi.ViewComponents
{
    public class VitrinViewModel
    {
        public List<Urun> Urunler { get; set; } = new();
        public List<Proje> Projeler { get; set; } = new();
    }

    public class VitrinViewComponent : ViewComponent
    {
        private readonly KurumsalDbContext _context;

        public VitrinViewComponent(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new VitrinViewModel
            {
                Urunler = await _context.Urunler
                    .OrderByDescending(u => u.Id)
                    .Take(6)
                    .AsNoTracking()
                    .ToListAsync(),

                Projeler = await _context.Projeler
                    .Where(p => p.AktifMi)
                    .OrderByDescending(p => p.Id)
                    .Take(6)
                    .AsNoTracking()
                    .ToListAsync()
            };

            return View(model);
        }
    }
}