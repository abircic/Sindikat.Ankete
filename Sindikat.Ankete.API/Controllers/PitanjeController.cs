using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sindikat.Ankete.Persistence;
using SindikatAnkete.Entity;
using Sindikat.Ankete.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Sindikat.Ankete.API.Controllers
{
    [Authorize(Policy = "StvoriAnketu")]
    [Route("api/[controller]")]
    [ApiController]
    public class PitanjeController : ControllerBase
    {
        private readonly AnketeDbContext _context;

        public PitanjeController(AnketeDbContext context)
        {
            _context = context;
        }

        // GET: api/Pitanje
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<PitanjeEntity>>> GetPitanja()
        //{
        //    return await _context.Pitanja.ToListAsync();
        //}

        [Authorize(Policy = "StvoriAnketu")]
        [HttpGet]
        public IQueryable<PitanjeDTO> GetPitanja()
        {
            var pitanje = from p in _context.Pitanja
                         select new PitanjeDTO()
                         {
                             TekstPitanja = p.TekstPitanja,
                             VrstaPitanja = p.TipPitanja.VrstaPitanja,
                             
                         };
            return pitanje;
        }



        // GET: api/Pitanje/5
        [Authorize(Policy = "StvoriAnketu")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PitanjeEntity>> GetPitanjeEntity(int id)
        {
            // var pitanjeEntity = await _context.Pitanja.FindAsync(id);
            //var query = _context.Ankete
            //     .Include(anketa => anketa.Pitanja)
            //         .ThenInclude(pitanje => pitanje.PonudeniOdgovori)
            //         .SingleOrDefault(anketa => anketa.Id == id);
            var query = _context.Pitanja
                .Include(t => t.TipPitanja)
                .Include(o => o.PonudeniOdgovori)
                .SingleOrDefault(pitanje => pitanje.Id == id);


            if (query == null)
            {
                return NotFound();
            }

            return Ok(query);
        }

        // PUT: api/Pitanje/5
        [Authorize(Policy = "StvoriAnketu")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPitanjeEntity(int id, PitanjeEntity pitanjeEntity)
        {
            var result = await _context.Pitanja.FindAsync(id);
            result.TekstPitanja = pitanjeEntity.TekstPitanja;
            _context.Pitanja.Update(result);
            _context.SaveChanges();
            return Ok(result);
        }

        // POST: api/Pitanje
        [Authorize(Policy = "StvoriAnketu")]
        [HttpPost]
        public async Task<ActionResult<PitanjeEntity>> PostPitanjeEntity(PitanjeEntity pitanjeEntity)
        {
            _context.Pitanja.Add(pitanjeEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPitanjeEntity", new { id = pitanjeEntity.Id }, pitanjeEntity);
        }

        // DELETE: api/Pitanje/5
        [Authorize(Policy = "StvoriAnketu")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PitanjeEntity>> DeletePitanjeEntity(int id)
        {
            var pitanjeEntity = await _context.Pitanja.FindAsync(id);
            if (pitanjeEntity == null)
            {
                return NotFound();
            }

            _context.Pitanja.Remove(pitanjeEntity);
            await _context.SaveChangesAsync();

            return pitanjeEntity;
        }

        private bool PitanjeEntityExists(int id)
        {
            return _context.Pitanja.Any(e => e.Id == id);
        }
    }
}
