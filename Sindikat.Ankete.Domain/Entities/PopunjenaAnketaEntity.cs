using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SindikatAnkete.Entity
{
    public class PopunjenaAnketaEntity
    {
        public int Id { get; set; }
        [Required]
        public string KorisnikId { get; set; }
        public AnketaEntity Anketa { get; set; }
        public ICollection<OdgovorEntity>Odgovori { get; set;}
    }
}
