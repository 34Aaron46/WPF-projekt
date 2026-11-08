using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuningKatalogusApi.Models
{
    public class Rendeles
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string VevoNev { get; set; } = "";

        [ForeignKey("Termek")]
        public int TermekId { get; set; }

        public int Darab { get; set; }

        [MaxLength(20)]
        public string Allapot { get; set; } = "Új";

        public Termek? Termek { get; set; }
    }
}
