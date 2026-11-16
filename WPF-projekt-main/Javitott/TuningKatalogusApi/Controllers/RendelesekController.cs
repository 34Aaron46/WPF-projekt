using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TuningKatalogusApi.Data;
using TuningKatalogusApi.Models;

namespace TuningKatalogusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RendelesekController : ControllerBase
    {
        private readonly AppDbContext db;

        // csak ezek az allapotok lehetnek
        private static readonly string[] AllapotLista = { "Új", "Feldolgozás alatt", "Kiszállítva", "Törölve" };

        public RendelesekController(AppDbContext db)
        {
            this.db = db;
        }

        // osszes rendeles a termekkel egyutt
        [HttpGet]
        public async Task<ActionResult<List<Rendeles>>> GetRendelesek()
        {
            return await db.Rendelesek.Include(r => r.Termek).ToListAsync();
        }

        // egy rendeles id alapjan
        [HttpGet("{id}")]
        public async Task<ActionResult<Rendeles>> GetRendeles(int id)
        {
            var rendeles = await db.Rendelesek
                .Include(r => r.Termek)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (rendeles == null)
            {
                return NotFound();
            }
            return rendeles;
        }

        // uj rendeles felvetele
        // ilyenkor levonja a keszletbol a megrendelt darabszamot
        [HttpPost]
        public async Task<ActionResult<Rendeles>> PostRendeles(Rendeles rendeles)
        {
            var termek = await db.Termekek.FindAsync(rendeles.TermekId);
            if (termek == null)
            {
                return BadRequest("Nincs ilyen termék!");
            }
            if (rendeles.Darab <= 0)
            {
                return BadRequest("A darabszám legyen nagyobb mint 0!");
            }
            if (termek.Keszlet < rendeles.Darab)
            {
                return BadRequest("Nincs elég készlet!");
            }
            termek.Keszlet -= rendeles.Darab;
            rendeles.Allapot = "Új";
            rendeles.Termek = null;
            db.Rendelesek.Add(rendeles);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRendeles), new { id = rendeles.Id }, rendeles);
        }

        // rendeles modositasa
        // meglevo rendelesnel csak a vevo neve es az allapot valtozhat
        // mert a termek es a darabszam a keszletet is erinti
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRendeles(int id, Rendeles rendeles)
        {
            var mentett = await db.Rendelesek.FindAsync(id);
            if (mentett == null)
            {
                return NotFound();
            }
            if (!AllapotLista.Contains(rendeles.Allapot))
            {
                return BadRequest("Ismeretlen állapot!");
            }
            mentett.VevoNev = rendeles.VevoNev;
            mentett.Allapot = rendeles.Allapot;
            await db.SaveChangesAsync();
            return NoContent();
        }

        // rendeles torlese
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRendeles(int id)
        {
            var rendeles = await db.Rendelesek.FindAsync(id);
            if (rendeles == null)
            {
                return NotFound();
            }
            db.Rendelesek.Remove(rendeles);
            await db.SaveChangesAsync();
            return NoContent();
        }

        // sajat fuggveny: csak az allapotot irja at
        [HttpPut("{id}/allapot/{allapot}")]
        public async Task<IActionResult> AllapotValtas(int id, string allapot)
        {
            var rendeles = await db.Rendelesek.FindAsync(id);
            if (rendeles == null)
            {
                return NotFound();
            }
            if (!AllapotLista.Contains(allapot))
            {
                return BadRequest("Ismeretlen állapot!");
            }
            rendeles.Allapot = allapot;
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}