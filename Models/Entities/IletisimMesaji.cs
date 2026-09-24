using System;
using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class IletisimMesaji
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string AdSoyad { get; set; }

        [Required]
        [MaxLength(150)]
        public string Eposta { get; set; }

        [MaxLength(50)]
        public string Telefon { get; set; }

        [MaxLength(200)]
        public string Konu { get; set; }

        [Required]
        public string Mesaj { get; set; }

        public bool OkunduMu { get; set; } = false;

        public DateTime GonderilmeTarihi { get; set; } = DateTime.Now;
    }
}