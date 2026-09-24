using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class Proje
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Baslik { get; set; }

        [MaxLength(100)]
        public string? Kategori { get; set; } = "Genel";

        public string? Icerik { get; set; }

        [MaxLength(500)]
        public string? KapakResmi { get; set; }

        public bool AktifMi { get; set; } = true;
    }
}