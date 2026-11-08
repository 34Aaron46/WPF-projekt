using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TuningKatalogusApi.Models
{
    public class Kategoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nev { get; set; } = "";

        [MaxLength(200)]
        public string? Leiras { get; set; }

        // a JsonIgnore miatt nem kerül bele a JSON-ba (így nincs végtelen ciklus)
        [JsonIgnore]
        public List<Termek> Termekek { get; set; } = new List<Termek>();
    }
}
