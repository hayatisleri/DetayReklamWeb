using KurumsalWebSitesi.Models;
using KurumsalWebSitesi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using BCrypt.Net;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// --- DOSYA VE VİDEO YÜKLEME LİMİTİNİ YÜKSELTME (150 MB) ---
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 157286400; // 150 MB
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 157286400; // 150 MB
});

// 1. MVC yapısını ekliyoruz
builder.Services.AddControllersWithViews();

// GSC API Servisini sisteme dahil ediyoruz
builder.Services.AddScoped<KurumsalWebSitesi.Services.GscApiService>();

// 2. Veritabanı bağlantı servisini (DbContext) ekliyoruz
builder.Services.AddDbContext<KurumsalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VarsayilanBaglanti")));

// 3. AUTHENTICATION (Oturum/Giriş) servislerini ekliyoruz
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Giris/Index";
        options.AccessDeniedPath = "/Admin/Giris/Index";
        options.Cookie.Name = "KurumsalAdminCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

// Canlıda tablolar yoksa otomatik olarak oluşturur
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<KurumsalWebSitesi.Models.KurumsalDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Olası bir bağlantı hatasını loglamak istersen
        Console.WriteLine("Veritabanı migration hatası: " + ex.Message);
    }
}

// Hata yönetimi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. AUTHENTICATION ve AUTHORIZATION middleware'leri
app.UseAuthentication();
app.UseAuthorization();

// Admin paneli için Area rota ayarı
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Standart ön yüz (kullanıcı) rota ayarı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- OTOMATİK ADMIN OLUŞTURMA (SEED) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<KurumsalDbContext>();

    if (!context.Kullanicilar.Any())
    {
        var admin = new KurumsalWebSitesi.Models.Entities.Kullanici
        {
            KullaniciAdi = "admin",
            SifreHash = BCrypt.Net.BCrypt.HashPassword("123456")
        };
        context.Kullanicilar.Add(admin);
        context.SaveChangesAsync().Wait();
    }
}

app.Run();