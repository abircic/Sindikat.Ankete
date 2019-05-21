using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SindikatAnkete.Entity
{
    public class PonudeniOdgovorEntity
    {
        public int Id { get; set; }
        [Required]
        public string DefiniraniOdgovor { get; set; }
        public PitanjeEntity Pitanje { get; set; }
    }
}
