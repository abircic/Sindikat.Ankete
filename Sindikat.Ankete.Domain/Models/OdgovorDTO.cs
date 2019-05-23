using System;
using System.Collections.Generic;
using System.Text;
using SindikatAnkete.Entity;

namespace Sindikat.Ankete.Domain.Models
{
    public class OdgovorDTO
    {
        public string OdgovorNaPitanje { get; set; }
        public int PitanjeId { get; set; }
    }
}
