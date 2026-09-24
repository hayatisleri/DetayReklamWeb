using KurumsalWebSitesi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;

namespace KurumsalWebSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GirisController : Controller
    {
        private readonly KurumsalDbContext _context;

        public GirisController(KurumsalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Eğer zaten giriş yapılmışsa, tekrar giriş sayfasına gönderme, panele at
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home", new { area = "Admin" });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string kullaniciAdi, string sifre)
        {
            // Kullanıcıyı veritabanında bul
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciAdi);

            // BCrypt.Verify ile şifreyi doğrula
            if (kullanici != null && BCrypt.Net.BCrypt.Verify(sifre, kullanici.SifreHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, kullanici.KullaniciAdi)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true }; // "Beni Hatırla" aktif

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }
    }
}