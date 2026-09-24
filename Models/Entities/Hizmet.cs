using System;
using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class Hizmet
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [MaxLength(200)]
        public string Baslik { get; set; }

        public string? KisaAciklama { get; set; }

        public string? Icerik { get; set; }

        public string? ResimAdresi { get; set; }

        public int Sira { get; set; } = 0;

        public bool AktifMi { get; set; } = true;

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;
    }
}