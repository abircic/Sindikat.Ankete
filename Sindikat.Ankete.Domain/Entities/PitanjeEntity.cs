using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SindikatAnkete.Entity
{
    public class PitanjeEntity
    {
        public int Id { get; set; }
        [Required]
        public string TekstPitanja { get; set; }
        public AnketaEntity Anketa { get; set; }
        public TipPitanjaEntity TipPitanja { get; set; }
        public ICollection<OdgovorEntity> Odgovori {get; set;}
        public ICollection<PonudeniOdgovorEntity> PonudeniOdgovori { get; set;}

    }
}
