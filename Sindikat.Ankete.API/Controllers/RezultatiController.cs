using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sindikat.Ankete.Persistence;
using SindikatAnkete.Entity;

namespace Sindikat.Ankete.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezultatiController : ControllerBase
    {
        private readonly AnketeDbContext _context;

        public RezultatiController(AnketeDbContext context)
        {
            _context = context;
        }

        // GET: api/Rezultati
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PopunjenaAnketaEntity>>> GetPopunjeneAnkete()
        {
            //var query = from p in _context.PopunjeneAnkete
            //            group p by p.KorisnikId into Group
            //            orderby Group.Key
            //            select Group;
            
            var query = from p in _context.PopunjeneAnkete
                        orderby p.KorisnikId
                        group p by p.KorisnikId into grp
                        select new
                        {
                            korisnik = grp.Key,
                            Broj_anketa = grp.Count()
                        };



            return Ok(query);
        }

        // GET: api/Rezultati/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PopunjenaAnketaEntity>> GetPopunjenaAnketaEntity(int id)
        {
            //var popunjenaAnketaEntity = await _context.PopunjeneAnkete.FindAsync(id);

            //if (popunjenaAnketaEntity == null)
            //{
            //    return NotFound();
            //}

            //return popunjenaAnketaEntity;
            var query = from p in _context.PopunjeneAnkete
                        where p.KorisnikId==id.ToString()
                        orderby p.KorisnikId
                        group p by p.KorisnikId into grp
                        select new
                        {
                            korisnik = grp.Key,
                            Broj_anketa = grp.Count()
                        };


            return Ok(query);

        }

        // PUT: api/Rezultati/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPopunjenaAnketaEntity(int id, PopunjenaAnketaEntity popunjenaAnketaEntity)
        {
            if (id != popunjenaAnketaEntity.AnketaId)
            {
                return BadRequest();
            }

            _context.Entry(popunjenaAnketaEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PopunjenaAnketaEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Rezultati
        [HttpPost]
        public async Task<ActionResult<PopunjenaAnketaEntity>> PostPopunjenaAnketaEntity(PopunjenaAnketaEntity popunjenaAnketaEntity)
        {
            _context.PopunjeneAnkete.Add(popunjenaAnketaEntity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PopunjenaAnketaEntityExists(popunjenaAnketaEntity.AnketaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPopunjenaAnketaEntity", new { id = popunjenaAnketaEntity.AnketaId }, popunjenaAnketaEntity);
        }

        // DELETE: api/Rezultati/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PopunjenaAnketaEntity>> DeletePopunjenaAnketaEntity(int id)
        {
            var popunjenaAnketaEntity = await _context.PopunjeneAnkete.FindAsync(id);
            if (popunjenaAnketaEntity == null)
            {
                return NotFound();
            }

            _context.PopunjeneAnkete.Remove(popunjenaAnketaEntity);
            await _context.SaveChangesAsync();

            return popunjenaAnketaEntity;
        }

        private bool PopunjenaAnketaEntityExists(int id)
        {
            return _context.PopunjeneAnkete.Any(e => e.AnketaId == id);
        }
    }
}
