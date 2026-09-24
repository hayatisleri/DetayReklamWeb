using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class IsBasvurusu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string AdSoyad { get; set; }
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }
        public string Pozisyon { get; set; }
        public string Mesaj { get; set; }
        public string CvDosyaYolu { get; set; } // CV dosyasını upload edip kaydedeceğiz
        public DateTime BasvuruTarihi { get; set; } = DateTime.Now;
    }
}