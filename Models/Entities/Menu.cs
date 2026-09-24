using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Menü başlığı zorunludur.")]
        [MaxLength(100)]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "Bağlantı adresi (URL) zorunludur.")]
        [MaxLength(250)]
        public string Url { get; set; }

        public int Sira { get; set; } = 99;

        // Dropdown (Açılır Menü) mantığı için kendi içine bağlanır. Eğer null ise ana menüdür.
        public int? UstMenuId { get; set; }

        public bool AktifMi { get; set; } = true;
    }
}