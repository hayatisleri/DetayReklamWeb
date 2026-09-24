using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;

namespace KurumsalWebSitesi.ViewComponents
{
    public class HeroViewComponent : ViewComponent
    {
        private readonly KurumsalDbContext _context;

        public HeroViewComponent(KurumsalDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 1. Üst alan için slaytları çekiyoruz
            var slaytlar = await _context.Slaytlar.Where(x => x.AktifMi).OrderBy(x => x.Sira).ToListAsync();

            // 2. Adminin eklediği öne çıkan kutuları çekip View'a gönderiyoruz
            var kutular = await _context.OneCikanKutular.Where(x => x.AktifMi).OrderBy(x => x.Sira).ToListAsync();
            ViewBag.OneCikanKutular = kutular;

            return View(slaytlar);
        }
    }
}