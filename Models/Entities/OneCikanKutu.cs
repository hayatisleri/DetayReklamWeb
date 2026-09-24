using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class OneCikanKutu
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(100)]
        public string Baslik { get; set; }

        [StringLength(100)]
        public string? Ikon { get; set; } = "bi bi-lightning-charge-fill";

        [StringLength(250)]
        public string? Link { get; set; } = "#cozumler";

        public int Sira { get; set; } = 1;

        public bool AktifMi { get; set; } = true;
    }
}