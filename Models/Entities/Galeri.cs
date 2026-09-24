using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class Galeri
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Baslik { get; set; }

        [MaxLength(500)]
        public string ResimAdresi { get; set; }
        public int Sira { get; set; }
    }
}