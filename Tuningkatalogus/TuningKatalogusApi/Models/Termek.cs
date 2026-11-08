using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuningKatalogusApi.Models
{
    public class Termek
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nev { get; set; } = "";

        [MaxLength(50)]
        public string? MuszakiAdat { get; set; }

        [Range(0, 100000000)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Ar { get; set; }

        [Range(0, 100000)]
        public int Keszlet { get; set; }

        [ForeignKey("Kategoria")]
        public int KategoriaId { get; set; }

        public Kategoria? Kategoria { get; set; }
    }
}
