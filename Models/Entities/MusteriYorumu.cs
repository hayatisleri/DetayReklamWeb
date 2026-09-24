using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class MusteriYorumu
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Müşteri veya şirket adı zorunludur.")]
        [StringLength(100)]
        public string AdSoyad { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Unvan { get; set; }

        [Required(ErrorMessage = "Yorum metni zorunludur.")]
        public string Yorum { get; set; } = string.Empty;

        public int Sira { get; set; } = 0;

        public bool AktifMi { get; set; } = true;
    }
}