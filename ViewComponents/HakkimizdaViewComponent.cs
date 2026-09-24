using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;

namespace KurumsalWebSitesi.ViewComponents
{
    public class HakkimizdaViewComponent : ViewComponent
    {
        private readonly KurumsalDbContext _context;

        public HakkimizdaViewComponent(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hakkimizda = await _context.Sayfalar
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SeoUrl == "hakkimizda" || s.Baslik.Contains("Hakkımızda"));

            return View(hakkimizda);
        }
    }
}