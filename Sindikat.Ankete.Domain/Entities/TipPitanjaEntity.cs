using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SindikatAnkete.Entity
{
    public class TipPitanjaEntity
    {
        public int Id { get; set; }
        [Required]
        public string VrstaPitanja { get; set; }
        //public ICollection<PitanjeEntity>Pitanja { get; set; } navodno ne triba
    }
}
