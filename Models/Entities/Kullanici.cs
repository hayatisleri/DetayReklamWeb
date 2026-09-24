using System.ComponentModel.DataAnnotations;


namespace KurumsalWebSitesi.Models.Entities
{

    public class Kullanici
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string KullaniciAdi { get; set; }

        [Required]
        public string SifreHash { get; set; } // Şifreyi asla açık metin tutma!
    }
}