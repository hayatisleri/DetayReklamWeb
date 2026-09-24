using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class Referans
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Firma adı zorunludur.")]
        [StringLength(150)]
        public string FirmaAdi { get; set; } = string.Empty;

        [StringLength(250)]
        public string? LogoAdresi { get; set; }

        public int Sira { get; set; } = 0;

        public bool AktifMi { get; set; } = true;
    }
}