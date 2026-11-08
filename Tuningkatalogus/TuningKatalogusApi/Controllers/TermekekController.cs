using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TuningKatalogusApi.Data;
using TuningKatalogusApi.Models;

namespace TuningKatalogusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TermekekController : ControllerBase
    {
        private readonly TuningContext db;

        public TermekekController(TuningContext db)
        {
            this.db = db;
        }

        // GET api/termekek
        [HttpGet]
        public async Task<ActionResult<List<Termek>>> GetTermekek()
        {
            return await db.Termekek.Include(t => t.Kategoria).ToListAsync();
        }

        // GET api/termekek/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Termek>> GetTermek(int id)
        {
            var termek = await db.Termekek
                .Include(t => t.Kategoria)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (termek == null)
            {
                return NotFound();
            }
            return termek;
        }

        // POST api/termekek
        [HttpPost]
        public async Task<ActionResult<Termek>> PostTermek(Termek termek)
        {
            bool vanKategoria = await db.Kategoriak.AnyAsync(k => k.Id == termek.KategoriaId);
            if (!vanKategoria)
            {
                return BadRequest("Nincs ilyen kategória!");
            }
            termek.Kategoria = null;
            db.Termekek.Add(termek);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTermek), new { id = termek.Id }, termek);
        }

        // PUT api/termekek/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTermek(int id, Termek termek)
        {
            if (id != termek.Id)
            {
                return BadRequest("Az id nem egyezik!");
            }
            bool letezik = await db.Termekek.AnyAsync(t => t.Id == id);
            if (!letezik)
            {
                return NotFound();
            }
            bool vanKategoria = await db.Kategoriak.AnyAsync(k => k.Id == termek.KategoriaId);
            if (!vanKategoria)
            {
                return BadRequest("Nincs ilyen kategória!");
            }
            termek.Kategoria = null;
            db.Entry(termek).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/termekek/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTermek(int id)
        {
            var termek = await db.Termekek.FindAsync(id);
            if (termek == null)
            {
                return NotFound();
            }
            // ha már rendelték, nem törölhetjük (idegen kulcs miatt)
            bool vanRendeles = await db.Rendelesek.AnyAsync(r => r.TermekId == id);
            if (vanRendeles)
            {
                return BadRequest("Ehhez a termékhez tartoznak rendelések!");
            }
            db.Termekek.Remove(termek);
            await db.SaveChangesAsync();
            return NoContent();
        }

        // saját függvény 1: keresés a termék nevében
        // GET api/termekek/kereses/turbo
        [HttpGet("kereses/{szoveg}")]
        public async Task<ActionResult<List<Termek>>> Kereses(string szoveg)
        {
            return await db.Termekek
                .Include(t => t.Kategoria)
                .Where(t => t.Nev.Contains(szoveg))
                .ToListAsync();
        }

        // saját függvény 2: készlet növelése
        // PUT api/termekek/1/keszlet/10
        [HttpPut("{id}/keszlet/{mennyiseg}")]
        public async Task<ActionResult<Termek>> KeszletNoveles(int id, int mennyiseg)
        {
            var termek = await db.Termekek.FindAsync(id);
            if (termek == null)
            {
                return NotFound();
            }
            if (mennyiseg <= 0)
            {
                return BadRequest("A mennyiség legyen nagyobb mint 0!");
            }
            termek.Keszlet += mennyiseg;
            await db.SaveChangesAsync();
            return termek;
        }
    }
}
