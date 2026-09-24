using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class EkipUyesi
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [MaxLength(150)]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Ünvan alanı zorunludur.")]
        [MaxLength(150)]
        public string Unvan { get; set; }

        public string ResimAdresi { get; set; }

        [MaxLength(250)]
        public string LinkedInAdresi { get; set; }

        [MaxLength(250)]
        public string TwitterAdresi { get; set; }

        [MaxLength(250)]
        public string InstagramAdresi { get; set; }

        public int Sira { get; set; } = 99;

        public bool AktifMi { get; set; } = true;
    }
}