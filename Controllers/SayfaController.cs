using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;
using System.Threading.Tasks;

namespace KurumsalWebSitesi.Controllers
{
    public class SayfaController : Controller
    {
        private readonly KurumsalDbContext _context;

        public SayfaController(KurumsalDbContext context)
        {
            _context = context;
        }

        [HttpGet("Sayfa/{seoUrl}")]
        public async Task<IActionResult> Detay(string seoUrl)
        {
            if (string.IsNullOrEmpty(seoUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            var sayfa = await _context.Sayfalar
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SeoUrl == seoUrl && s.AktifMi);

            // Veritabanında sayfa bulamazsa, koddan otomatik oluştur!
            if (sayfa == null)
            {
                if (seoUrl.ToLower() == "hakkimizda")
                {
                    sayfa = new Sayfa
                    {
                        Baslik = "Hakkımızda",
                        SeoUrl = "hakkimizda",
                        Icerik = @"<p>Sektördeki yenilikçi vizyonumuz ve yıllara dayanan üretim tecrübemizle, kurumsal markaların sahada en güçlü şekilde temsil edilmesini sağlıyoruz. İleri teknoloji lazer ve CNC kesim parkurumuz, yüksek çözünürlüklü dijital baskı makinelerimiz ve uzman montaj ekibimizle; ışıktan pleksiye, dış cephe giydirmeden araç kaplamaya kadar tüm reklam ihtiyaçlarını tek çatı altında karşılıyoruz.</p>
                                   <p><strong>Neler Yapıyoruz?</strong></p>
                                   <ul>
                                     <li><strong>Tabela Sistemleri:</strong> Pleksi, paslanmaz krom, alüminyum fileli kutu harf ve kompozit oyma ışıklı tabela çözümleri.</li>
                                     <li><strong>Araç Giydirme:</strong> Ticari filolar ve münferit araçlar için UV korumalı, uzun ömürlü dökme folyo kaplama uygulamaları.</li>
                                     <li><strong>Dijital Baskı & Üretim:</strong> İç ve dış mekan baskı, one way vision, yönlendirme levhaları ve display ürünler.</li>
                                   </ul>
                                   <p>Projelendirme aşamasından montaj ve teslimat sonrasına kadar sıfır hata toleransı, kaliteli malzeme ve zamanında teslimat prensibiyle iş ortaklarımızın çözüm ortağı olmaktan gurur duyuyoruz.</p>"
                    };
                }
                else
                {
                    sayfa = new Sayfa
                    {
                        Baslik = "Kurumsal Sayfa",
                        SeoUrl = seoUrl,
                        Icerik = "<p>Bu sayfa içeriği henüz hazırlanmaktadır.</p>"
                    };
                }
            }

            return View(sayfa);
        }
    }
}