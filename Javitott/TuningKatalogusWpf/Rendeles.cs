namespace TuningKatalogusWpf.Models
{
    public class Rendeles
    {
        public int Id { get; set; }
        public string VevoNev { get; set; } = "";
        public int TermekId { get; set; }
        public int Darab { get; set; }
        public string Allapot { get; set; } = "Új";
        public Termek? Termek { get; set; }

        // a rendeles vegosszege ez jelenik meg a tablazatban
        public decimal Osszeg
        {
            get
            {
                if (Termek == null)
                {
                    return 0;
                }
                return Termek.Ar * Darab;
            }
        }
    }
}