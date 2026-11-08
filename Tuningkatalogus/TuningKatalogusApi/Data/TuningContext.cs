using Microsoft.EntityFrameworkCore;
using TuningKatalogusApi.Models;

namespace TuningKatalogusApi.Data
{
    public class TuningContext : DbContext
    {
        public TuningContext(DbContextOptions<TuningContext> options) : base(options)
        {
        }

        public DbSet<Kategoria> Kategoriak { get; set; }
        public DbSet<Termek> Termekek { get; set; }
        public DbSet<Rendeles> Rendelesek { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // kezdő adatok, hogy ne üres legyen az adatbázis
            modelBuilder.Entity<Kategoria>().HasData(
                new Kategoria { Id = 1, Nev = "Turbó", Leiras = "Turbófeltöltők" },
                new Kategoria { Id = 2, Nev = "Motor belső", Leiras = "Dugattyú, hajtókar, csapágy" },
                new Kategoria { Id = 3, Nev = "Kipufogó", Leiras = "Leömlő, straight pipe" }
            );

            modelBuilder.Entity<Termek>().HasData(
                new Termek { Id = 1, Nev = "Garrett G30-770 turbó", MuszakiAdat = "+140 LE", Ar = 489000, Keszlet = 5, KategoriaId = 1 },
                new Termek { Id = 2, Nev = "Kovácsolt dugattyú szett", MuszakiAdat = "11,5 bar", Ar = 238000, Keszlet = 6, KategoriaId = 2 },
                new Termek { Id = 3, Nev = "Versenycsapágy szett", MuszakiAdat = "0,025 mm", Ar = 68000, Keszlet = 12, KategoriaId = 2 },
                new Termek { Id = 4, Nev = "Tuning leömlő 4-2-1", MuszakiAdat = "+12 LE", Ar = 179000, Keszlet = 4, KategoriaId = 3 },
                new Termek { Id = 5, Nev = "Straight pipe 76 mm", MuszakiAdat = "76 mm", Ar = 94000, Keszlet = 9, KategoriaId = 3 }
            );
        }
    }
}
