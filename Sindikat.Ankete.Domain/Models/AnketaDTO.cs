using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Ankete.Domain.Models
{
    public class AnketaDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public DateTime VrijemeKreiranja { get; set; }
        public string Opis { get; set; }
        public virtual ICollection<PitanjeDTO> PitanjeDTO { get; set; }
    }
}
