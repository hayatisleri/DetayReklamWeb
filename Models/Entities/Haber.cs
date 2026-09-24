using System;
using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class Haber
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Baslik { get; set; }

        [Required]
        [MaxLength(250)]
        public string SeoUrl { get; set; }

        [MaxLength(500)]
        public string Ozet { get; set; }

        public string Icerik { get; set; }

        [MaxLength(500)]
        public string ResimAdresi { get; set; }

        public bool AktifMi { get; set; } = true;

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;
    }
}