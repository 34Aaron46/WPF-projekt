using Microsoft.EntityFrameworkCore;
using TuningKatalogusApi.Models;

namespace TuningKatalogusApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ezekbol lesznek a tablak
        public DbSet<Kategoria> Kategoriak { get; set; }
        public DbSet<Termek> Termekek { get; set; }
        public DbSet<Rendeles> Rendelesek { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // kezdo adatok hogy ne legyen ures az adatbazis
            modelBuilder.Entity<Kategoria>().HasData(
                new Kategoria { Id = 1, Nev = "Turbó", Leiras = "Turbófeltöltők és kiegészítők" },
                new Kategoria { Id = 2, Nev = "Motor belső", Leiras = "Dugattyú, hajtókar, csapágy" },
                new Kategoria { Id = 3, Nev = "Kipufogó", Leiras = "Leömlő, katalizátor, hangtompító" },
                new Kategoria { Id = 4, Nev = "Szívórendszer", Leiras = "Légszűrő, szívócső" },
                new Kategoria { Id = 5, Nev = "Futómű", Leiras = "Csavarmenetes futómű, stabilizátor" },
                new Kategoria { Id = 6, Nev = "Fékrendszer", Leiras = "Féknyereg, fékbetét, féktárcsa" },
                new Kategoria { Id = 7, Nev = "Chip tuning", Leiras = "Motorvezérlő szoftverek" }
            );

            modelBuilder.Entity<Termek>().HasData(
                new Termek { Id = 1, Nev = "Garrett G30-770 turbó", MuszakiAdat = "+140 LE", Ar = 489000, Keszlet = 5, KategoriaId = 1 },
                new Termek { Id = 2, Nev = "Kovácsolt dugattyú szett", MuszakiAdat = "11,5 bar", Ar = 238000, Keszlet = 6, KategoriaId = 2 },
                new Termek { Id = 3, Nev = "Versenycsapágy szett", MuszakiAdat = "0,025 mm", Ar = 68000, Keszlet = 12, KategoriaId = 2 },
                new Termek { Id = 4, Nev = "Tuning leömlő 4-2-1", MuszakiAdat = "+12 LE", Ar = 179000, Keszlet = 4, KategoriaId = 3 },
                new Termek { Id = 5, Nev = "Straight pipe 76 mm", MuszakiAdat = "76 mm", Ar = 94000, Keszlet = 9, KategoriaId = 3 },
                new Termek { Id = 6, Nev = "BorgWarner EFR 7163 turbó", MuszakiAdat = "+180 LE", Ar = 620000, Keszlet = 3, KategoriaId = 1 },
                new Termek { Id = 7, Nev = "Front mount intercooler", MuszakiAdat = "-25 °C", Ar = 135000, Keszlet = 7, KategoriaId = 1 },
                new Termek { Id = 8, Nev = "Wastegate szelep 40 mm", MuszakiAdat = "40 mm", Ar = 52000, Keszlet = 10, KategoriaId = 1 },
                new Termek { Id = 9, Nev = "Kovácsolt hajtókar szett", MuszakiAdat = "1000 LE-ig", Ar = 310000, Keszlet = 3, KategoriaId = 2 },
                new Termek { Id = 10, Nev = "Olajhűtő szett", MuszakiAdat = "13 soros", Ar = 46000, Keszlet = 8, KategoriaId = 2 },
                new Termek { Id = 11, Nev = "Sportkatalizátor", MuszakiAdat = "200 cellás", Ar = 85000, Keszlet = 6, KategoriaId = 3 },
                new Termek { Id = 12, Nev = "Sport hátsó hangtompító", MuszakiAdat = "76 mm", Ar = 72000, Keszlet = 7, KategoriaId = 3 },
                new Termek { Id = 13, Nev = "Sport légszűrő", MuszakiAdat = "+5 LE", Ar = 24000, Keszlet = 20, KategoriaId = 4 },
                new Termek { Id = 14, Nev = "Szívócső szett", MuszakiAdat = "+8 LE", Ar = 58000, Keszlet = 6, KategoriaId = 4 },
                new Termek { Id = 15, Nev = "Csavarmenetes futómű", MuszakiAdat = "30-60 mm süllyesztés", Ar = 215000, Keszlet = 5, KategoriaId = 5 },
                new Termek { Id = 16, Nev = "Stabilizátor rúd szett", MuszakiAdat = "26 mm", Ar = 39000, Keszlet = 9, KategoriaId = 5 },
                new Termek { Id = 17, Nev = "Négydugattyús féknyereg szett", MuszakiAdat = "330 mm tárcsa", Ar = 295000, Keszlet = 4, KategoriaId = 6 },
                new Termek { Id = 18, Nev = "Sport fékbetét", MuszakiAdat = "Racing", Ar = 32000, Keszlet = 15, KategoriaId = 6 },
                new Termek { Id = 19, Nev = "Chip tuning Stage 1", MuszakiAdat = "+40 LE", Ar = 120000, Keszlet = 30, KategoriaId = 7 },
                new Termek { Id = 20, Nev = "Chip tuning Stage 2", MuszakiAdat = "+70 LE", Ar = 180000, Keszlet = 20, KategoriaId = 7 }
            );

            modelBuilder.Entity<Rendeles>().HasData(
                new Rendeles { Id = 1, VevoNev = "Kovács Bence", TermekId = 1, Darab = 1, Allapot = "Új" },
                new Rendeles { Id = 2, VevoNev = "Nagy Dávid", TermekId = 13, Darab = 2, Allapot = "Kiszállítva" },
                new Rendeles { Id = 3, VevoNev = "Szabó Márk", TermekId = 15, Darab = 1, Allapot = "Feldolgozás alatt" },
                new Rendeles { Id = 4, VevoNev = "Tóth Gergő", TermekId = 18, Darab = 2, Allapot = "Kiszállítva" },
                new Rendeles { Id = 5, VevoNev = "Horváth Ádám", TermekId = 19, Darab = 1, Allapot = "Új" },
                new Rendeles { Id = 6, VevoNev = "Varga Levente", TermekId = 6, Darab = 1, Allapot = "Törölve" }
            );
        }
    }
}