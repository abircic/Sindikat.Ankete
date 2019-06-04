using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "IspuniAnketu")]
        [HttpGet("/api/[controller]/ObradaAnketeUlogiranogKorisnika")]
        public async Task<ActionResult<PopunjenaAnketaEntity>> ObradaAnketeUlogiranogKorisnika()
        {
            var KorisnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = from o in _context.Odgovori
                        join p in _context.Pitanja on o.PitanjeId equals p.Id
                        join a in _context.Ankete on p.Anketa.Id equals a.Id
                        where o.KorisnikId == KorisnikId
                        orderby o.PitanjeId
                        group o by new { a.Id, o.OdgovorPitanja, o.PitanjeId, p.TekstPitanja } into grp
                        select new
                        {
                            AnketaId = grp.Key.Id,
                            idPitanje = grp.Key.PitanjeId,
                            tekst_pitanja = grp.Key.TekstPitanja,
                            odgovor = grp.Key.OdgovorPitanja
                        };

            return Ok(query);
        }
        // GET: api/Rezultati/5
        [Authorize(Policy = "IspuniAnketu")]
        [HttpGet("BrojPopunjenihAnketaZaUlogiranogKorisnika")]
        public async Task<ActionResult<PopunjenaAnketaEntity>> BrojAnketaZaUlogiranogKorisnika()
        {
            string KorisnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = from p in _context.PopunjeneAnkete
                        where p.KorisnikId == KorisnikId
                        orderby p.KorisnikId
                        group p by p.KorisnikId into grp
                        select new
                        {
                            korisnik = grp.Key,
                            Broj_popunjenih_anketa_korisnika = grp.Count()
                        };


            return Ok(query);

        }
        // GET: api/Rezultati
        [Authorize(Policy = "Rezultati")]
        [HttpGet("BrojUkupnoPopunjenihAnketa")]
        public async Task<ActionResult<IEnumerable<PopunjenaAnketaEntity>>> UkupanBrojAnketa()
        {
          var query = from p in _context.PopunjeneAnkete
                        orderby p.AnketaId
                        group p by p.AnketaId into grp
                        select new
                        {
                            AnketaId = grp.Key,
                            Broj_anketa = grp.Count()
                        };



            return Ok(query);
        }
        [Authorize(Policy = "Rezultati")]
        [HttpGet("BrojUkupnoPopunjenihAnketaPoId/{id}")]
        public async Task<ActionResult<IEnumerable<PopunjenaAnketaEntity>>> UkupanBrojAnketaId(int id)
        {
            var query = from p in _context.PopunjeneAnkete
                        orderby p.AnketaId
                        where p.AnketaId == id
                        group p by p.AnketaId into grp
                        select new
                        {
                            AnketaId = grp.Key,
                            Broj_anketa = grp.Count()
                        };



            return Ok(query);
        }
        [Authorize(Policy = "Rezultati")]
        [HttpGet("/api/[controller]/ObradaAnkete/{id}")]
        public async Task<ActionResult<PopunjenaAnketaEntity>> ObradaAnkete(int id)
        {
            var query2 = from p in _context.Odgovori
                         orderby p.PitanjeId
                         group p by p.PitanjeId into grp
                         select new
                         {
                             id = grp.Key,
                             pitanje = grp.Count()
                         };

            var query = from o in _context.Odgovori
                        join p in _context.Pitanja on o.PitanjeId equals p.Id
                        from q in query2
                        where o.PitanjeId == q.id && p.Anketa.Id==id
                        orderby o.PitanjeId
                        group o by new { o.OdgovorPitanja, o.PitanjeId, q.pitanje} into grp
                        select new
                        {
                            idAnkete = id,
                            idPitanje = grp.Key.PitanjeId,
                            odgovor = grp.Key.OdgovorPitanja,
                            broj_odgovora = grp.Count(),
                            broj_ukupnih_odgovora_na_pitanje = grp.Key.pitanje,
                            postotak = ((((float)grp.Count())/((float)(grp.Key.pitanje)))*100).ToString("0.00") + "%"


                        };
           
            return Ok(query);
        }

       
        // GET: api/Rezultati/5
        [Authorize(Policy = "Rezultati")]
        [HttpGet("BrojPopunjenihAnketaZaIdKorisnika/{id}")]
        public async Task<ActionResult<PopunjenaAnketaEntity>> BrojAnketaZaOdredenogKorisnikaAdmin(int id)
        {
            var query = from p in _context.PopunjeneAnkete
                        where p.KorisnikId==id.ToString()
                        orderby p.KorisnikId
                        group p by p.KorisnikId into grp
                        select new
                        {
                            korisnik = grp.Key,
                            Broj_popunjenih_anketa_korisnika = grp.Count()
                        };


            return Ok(query);

        }
        
        [Authorize(Policy = "Rezultati")]
        [HttpGet("ObradaAnketePoPitanju/{idAnkete}/{idPitanja}")]
        public async Task<ActionResult<PopunjenaAnketaEntity>> RezultatPitanjaPoPitanju(int idAnkete, int idPitanja)
        {
            var query2 = from o in _context.Odgovori
                         orderby o.PitanjeId
                         where o.Pitanje.Anketa.Id == idAnkete && o.PitanjeId == idPitanja
                         group o by o.PitanjeId into grp
                         select new
                         {
                             id = grp.Key,
                             pitanje = grp.Count()
                         };
            var query = from o in _context.Odgovori
                        join p in _context.Pitanja on o.PitanjeId equals p.Id
                        join a in _context.Ankete on p.Anketa.Id equals a.Id
                        from q in query2
                        where o.PitanjeId == idPitanja && p.Anketa.Id == idAnkete
                        group o by new { o.OdgovorPitanja, q.pitanje } into grp
                        select new
                        {
                            Odgovor = grp.Key.OdgovorPitanja,
                            Broj_odgovora = grp.Count(),
                            broj_ukupnih_odgovora_na_pitanje = grp.Key.pitanje,
                            postotak = ((((float)grp.Count()) / ((float)(grp.Key.pitanje))) * 100).ToString("0.00") + "%"
                        };

            return Ok(query);
        }
        [Authorize(Policy = "Rezultati")]
        [HttpGet("ObradaAnketePoOdgovoru/{idAnkete}/{idPitanja}/{odgovor}")]
        public async Task<ActionResult<PopunjenaAnketaEntity>> RezultatPitanjaPoOdgovoru(int idAnkete, int idPitanja, string odgovor)
        {
            var query2 = from o in _context.Odgovori
                         orderby o.PitanjeId
                         where o.Pitanje.Anketa.Id==idAnkete && o.PitanjeId==idPitanja
                         group o by o.PitanjeId into grp
                         select new
                         {
                             id = grp.Key,
                             pitanje = grp.Count()
                         };
            var query = from o in _context.Odgovori
                        join p in _context.Pitanja on o.PitanjeId equals p.Id
                        join a in _context.Ankete on p.Anketa.Id equals a.Id
                        from q in query2
                        where o.PitanjeId==idPitanja && p.Anketa.Id==idAnkete && o.OdgovorPitanja==odgovor
                        group o by new { o.OdgovorPitanja, q.pitanje } into grp
                        select new
                        {
                            Odgovor = grp.Key.OdgovorPitanja,
                            Broj_odgovora = grp.Count(),
                            broj_ukupnih_odgovora_na_pitanje=grp.Key.pitanje,
                            postotak = ((((float)grp.Count()) / ((float)(grp.Key.pitanje))) * 100).ToString("0.00") + "%"
                        };
                           
            return Ok(query);
        }

        private bool PopunjenaAnketaEntityExists(int id)
        {
            return _context.PopunjeneAnkete.Any(e => e.AnketaId == id);
        }
    }
}
