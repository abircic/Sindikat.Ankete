using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SindikatAnkete.Entity
{
    public class OdgovorEntity
    {
        public int PitanjeId { get; set; }
        [Required]
        public string OdgovorPitanja { get; set; }
        public PitanjeEntity Pitanje { get; set; }
        public string KorisnikId { get; set; }
    }
}
