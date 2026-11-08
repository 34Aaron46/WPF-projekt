namespace TuningKatalogusWpf.Models
{
    public class Termek
    {
        public int Id { get; set; }
        public string Nev { get; set; } = "";
        public string? MuszakiAdat { get; set; }
        public decimal Ar { get; set; }
        public int Keszlet { get; set; }
        public int KategoriaId { get; set; }
        public Kategoria? Kategoria { get; set; }
    }
}
