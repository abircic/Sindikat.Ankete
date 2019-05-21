using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Ankete.Domain.Models
{
    public class PitanjeDTO
    {
        public string TekstPitanja { get; set; }
        public string VrstaPitanja { get; set; }
       // public ICollection<string> PonudeniOdgovori { get; set; }
       public ICollection<string> ponudeniOdgovori { get; set; }
    }
}
