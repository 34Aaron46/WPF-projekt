using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TuningKatalogusApi.Data;
using TuningKatalogusApi.Models;

namespace TuningKatalogusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RendelesekController : ControllerBase
    {
        private readonly TuningContext db;

        // ezek az állapotok engedélyezettek
        private static readonly string[] AllapotLista = { "Új", "Feldolgozás alatt", "Kiszállítva", "Törölve" };

        public RendelesekController(TuningContext db)
        {
            this.db = db;
        }

        // GET api/rendelesek
        [HttpGet]
        public async Task<ActionResult<List<Rendeles>>> GetRendelesek()
        {
            return await db.Rendelesek.Include(r => r.Termek).ToListAsync();
        }

        // GET api/rendelesek/1
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

        // POST api/rendelesek
        // új rendelésnél levonjuk a készletből a megrendelt darabszámot
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

        // PUT api/rendelesek/1
        // meglévő rendelésnél csak a vevő neve és az állapot módosítható,
        // mert a termék és a darabszám a készletet is érinti
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

        // DELETE api/rendelesek/1
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

        // saját függvény: csak az állapotot írja át
        // PUT api/rendelesek/1/allapot/Kiszállítva
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
