using System;
using System.ComponentModel.DataAnnotations;

namespace KurumsalWebSitesi.Models.Entities
{
    public class Slayt
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Baslik { get; set; }

        [MaxLength(200)]
        public string AltBaslik { get; set; }

        [Required]
        [MaxLength(500)]
        public string ResimAdresi { get; set; }

        [MaxLength(500)]
        public string LinkAdresi { get; set; } // Slayta tıklanınca gidecek adres

        public int Sira { get; set; } = 0;

        public bool AktifMi { get; set; } = true;
    }
}