using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SindikatAnkete.Entity
{
    public class AnketaEntity
    {
        public int Id { get; set; }
        [Required]
        public string Naziv { get; set; }
        public DateTime VrijemeKreiranja { get; set; }
        public string Opis { get; set; }
        public bool status { get; set; }
        public virtual ICollection<PopunjenaAnketaEntity> PopunjeneAnkete { get; set; }
        public virtual ICollection<PitanjeEntity> Pitanja { get; set; }
    }
}
