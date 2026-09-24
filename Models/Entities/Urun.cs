using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class Urun
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Ad { get; set; }

        [MaxLength(100)]
        public string? Kategori { get; set; } = "Genel";

        public string? Aciklama { get; set; }

        [MaxLength(500)]
        public string? ResimAdresi { get; set; }

        public string? TeknikDetaylar { get; set; }
    }
}