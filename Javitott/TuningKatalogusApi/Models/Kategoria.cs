using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TuningKatalogusApi.Models
{
    public class Kategoria
    {
        // ez az elsodleges kulcs
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nev { get; set; } = "";

        [MaxLength(200)]
        public string? Leiras { get; set; }

        // ezt nem kell a jsonba mert kulonben vegtelen ciklus lesz
        [JsonIgnore]
        public List<Termek> Termekek { get; set; } = new List<Termek>();
    }
}