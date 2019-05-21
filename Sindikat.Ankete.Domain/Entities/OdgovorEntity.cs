using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SindikatAnkete.Entity
{
    public class OdgovorEntity
    {
        public int Id { get; set; }
        [Required]
        public string OdgovorPitanja { get; set; }
        public PopunjenaAnketaEntity PopunjenaAnketa { get; set; }
    }
}
