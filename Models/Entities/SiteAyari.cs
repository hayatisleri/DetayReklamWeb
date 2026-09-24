using System;
using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class SiteAyari
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SiteBasligi { get; set; }

        [MaxLength(250)]
        public string SiteAciklamasi { get; set; }

        [MaxLength(500)]
        public string LogoAdresi { get; set; }

        [MaxLength(500)]
        public string FaviconAdresi { get; set; }

        [MaxLength(100)]
        public string TelefonNumarasi { get; set; }

        [MaxLength(100)]
        public string EpostaAdresi { get; set; }

        [MaxLength(250)]
        public string Adres { get; set; }

        [MaxLength(250)]
        public string FacebookAdresi { get; set; }

        [MaxLength(250)]
        public string? WhatsappNumarasi { get; set; }

        [MaxLength(250)]
        public string InstagramAdresi { get; set; }

        [MaxLength(250)]
        public string LinkedInAdresi { get; set; }

        // Analiz ve İstatistik Alanları
        [MaxLength(50)]
        public string GoogleAnalyticsId { get; set; }

        // Google Search Console API Alanları
        [MaxLength(250)]
        public string GscPropertyUrl { get; set; }

        [MaxLength(500)]
        public string GscServiceAccountJsonPath { get; set; }

        public DateTime GuncellenmeTarihi { get; set; } = DateTime.Now;
    }
}