using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TuningKatalogusApi.Data;
using TuningKatalogusApi.Models;

namespace TuningKatalogusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KategoriakController : ControllerBase
    {
        private readonly AppDbContext db;

        public KategoriakController(AppDbContext db)
        {
            this.db = db;
        }

        // osszes kategoria
        [HttpGet]
        public async Task<ActionResult<List<Kategoria>>> GetKategoriak()
        {
            return await db.Kategoriak.ToListAsync();
        }

        // egy kategoria id alapjan
        [HttpGet("{id}")]
        public async Task<ActionResult<Kategoria>> GetKategoria(int id)
        {
            var kategoria = await db.Kategoriak.FindAsync(id);
            if (kategoria == null)
            {
                return NotFound();
            }
            return kategoria;
        }

        // uj kategoria felvetele
        [HttpPost]
        public async Task<ActionResult<Kategoria>> PostKategoria(Kategoria kategoria)
        {
            db.Kategoriak.Add(kategoria);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetKategoria), new { id = kategoria.Id }, kategoria);
        }

        // kategoria modositasa
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKategoria(int id, Kategoria kategoria)
        {
            if (id != kategoria.Id)
            {
                return BadRequest("Az id nem egyezik!");
            }
            bool letezik = await db.Kategoriak.AnyAsync(k => k.Id == id);
            if (!letezik)
            {
                return NotFound();
            }
            db.Entry(kategoria).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return NoContent();
        }

        // kategoria torlese
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKategoria(int id)
        {
            var kategoria = await db.Kategoriak.FindAsync(id);
            if (kategoria == null)
            {
                return NotFound();
            }
            // ha van benne termek akkor nem lehet torolni
            bool vanTermek = await db.Termekek.AnyAsync(t => t.KategoriaId == id);
            if (vanTermek)
            {
                return BadRequest("A kategóriában még vannak termékek!");
            }
            db.Kategoriak.Remove(kategoria);
            await db.SaveChangesAsync();
            return NoContent();
        }

        // sajat fuggveny: egy kategoria osszes termeke
        [HttpGet("{id}/termekek")]
        public async Task<ActionResult<List<Termek>>> GetKategoriaTermekei(int id)
        {
            bool letezik = await db.Kategoriak.AnyAsync(k => k.Id == id);
            if (!letezik)
            {
                return NotFound();
            }
            return await db.Termekek.Where(t => t.KategoriaId == id).ToListAsync();
        }
    }
}