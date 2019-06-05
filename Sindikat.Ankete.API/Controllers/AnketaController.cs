using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sindikat.Ankete.Domain.Models;
using Sindikat.Ankete.Persistence;
using SindikatAnkete.Entity;

namespace Sindikat.Ankete.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AnketaController : ControllerBase
    {
        private readonly AnketeDbContext _context;

        public AnketaController(AnketeDbContext context)
        {
            _context = context;
        }
        [Authorize(Policy = "IspuniAnketu")]
        [HttpGet]
        public IQueryable<AnketaMenuDTO> GetAnkete()
        {
            var anketa = from a in _context.Ankete
                         where a.status.Equals(true)
                         select new AnketaMenuDTO()
                         {
                             Id = a.Id,
                             Naziv = a.Naziv,
                             Opis = a.Opis,
                             VrijemeKreiranja = a.VrijemeKreiranja
                         };
            return anketa;
        }

        // GET: api/Anketa/5
        [Authorize(Policy = "IspuniAnketu")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AnketaEntity>> GetAnketaEntity(int id)
        {
            //    var anketaEntity = await _context.Ankete.FindAsync(id);

            var query = _context.Ankete
                .Include(anketa => anketa.Pitanja)
                    .ThenInclude(pitanje => pitanje.PonudeniOdgovori)
                    .SingleOrDefault(anketa => anketa.Id == id);


            if (query == null)
            {
                return NotFound();
            }

            return Ok(query);
        }


        // PUT: api/Anketa/5
        [Authorize(Policy = "StvoriAnketu")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnketaEntity(int id, bool status)
        {
            var result = await _context.Ankete.FindAsync(id);

            result.status = status;
            _context.Ankete.Update(result);
            _context.SaveChanges();
            return Ok(result);
        }

        // POST: api/Anketa
        [Authorize(Policy = "StvoriAnketu")]
        [HttpPost]
        public async Task<ActionResult<AnketaEntity>> PostAnketaEntity(AnketaDTO anketaDTO)
        {
            
            AnketaEntity anketa = new AnketaEntity();
            anketa.status = true;
            anketa.Naziv = anketaDTO.Naziv;
            anketa.VrijemeKreiranja = anketaDTO.VrijemeKreiranja;
            anketa.Opis = anketaDTO.Opis;
            List<PitanjeEntity> listaPitanja = new List<PitanjeEntity>();
            foreach (var pitanje in anketaDTO.PitanjeDTO)
            {
                var p = new PitanjeEntity();
                p.TekstPitanja = pitanje.TekstPitanja;
                p.TipPitanja = new TipPitanjaEntity();
                var query = _context.TipoviPitanja.SingleOrDefault(tip => tip.VrstaPitanja == pitanje.VrstaPitanja);
                if (query == null)
                {
                    p.TipPitanja.VrstaPitanja = pitanje.VrstaPitanja;
                }
                else
                {
                    p.TipPitanja.VrstaPitanja = query.VrstaPitanja;
                    p.TipPitanja.Id = query.Id;
                }
                //p.TipPitanja.VrstaPitanja = pitanje.VrstaPitanja;
                p.PonudeniOdgovori = new List<PonudeniOdgovorEntity>();
                foreach (var odgovor in pitanje.ponudeniOdgovori)
                {
                    var ponudeniOdgovor = new PonudeniOdgovorEntity();
                    ponudeniOdgovor.DefiniraniOdgovor = odgovor;
                    p.PonudeniOdgovori.Add(ponudeniOdgovor);
                }
                listaPitanja.Add(p);
                anketa.Pitanja = listaPitanja;
                //await _context.Pitanja.AddAsync(p);
                //_context.SaveChanges();
            }
            await _context.Ankete.AddAsync(anketa);
            _context.SaveChanges();
            return Ok("Kreirana je nova anketa! ");
        }

        // DELETE: api/Anketa/5
        [Authorize(Policy = "StvoriAnketu")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnketaEntity>> DeleteAnketaEntity(int id)
        {
            var anketaEntity = await _context.Ankete.FindAsync(id);
            if (anketaEntity == null)
            {
                return NotFound();
            }

            _context.Ankete.Remove(anketaEntity);
            await _context.SaveChangesAsync();

            return anketaEntity;
        }

        private bool AnketaEntityExists(int id)
        {
            return _context.Ankete.Any(e => e.Id == id);
        }
        [Authorize(Policy = "IspuniAnketu")]
        [HttpPost("/api/[controller]/ispuni")]
        public async Task<ActionResult<IspuniAnketuDTO>> PostAnketaEntity(IspuniAnketuDTO ispuniAnketu)
        {
            PopunjenaAnketaEntity popunjenaAnketa = new PopunjenaAnketaEntity();
            popunjenaAnketa.AnketaId = ispuniAnketu.AnketaId;
            popunjenaAnketa.KorisnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var anketa = _context.Ankete.Where(a => a.Id == ispuniAnketu.AnketaId).SingleOrDefault();
            popunjenaAnketa.Anketa = anketa;


            List<OdgovorEntity> listaOdgovora = new List<OdgovorEntity>();
            foreach(var odgovor in ispuniAnketu.Odgovor)
            {
                var odg = new OdgovorEntity();
                odg.OdgovorPitanja = odgovor.OdgovorNaPitanje;
                odg.KorisnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                odg.PitanjeId = odgovor.Pitanje;
                listaOdgovora.Add(odg);
            }

            
            await _context.PopunjeneAnkete.AddAsync(popunjenaAnketa);
            await _context.Odgovori.AddRangeAsync(listaOdgovora);
            await _context.SaveChangesAsync();

 

            return Ok("Pohranjeni odgovori na anketu");


        }
    }
}