using System;
using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class Sayfa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Baslik { get; set; }

        [Required]
        [MaxLength(250)]
        public string SeoUrl { get; set; } // SEO uyumlu URL için (örneğin: /hakkimizda)

        public string Icerik { get; set; } // HTML formatında sayfa içeriği

        public bool AktifMi { get; set; } = true;

        [MaxLength(200)]
        public string SeoBasligi { get; set; }

        [MaxLength(500)]
        public string SeoAciklamasi { get; set; }

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;
    }
}