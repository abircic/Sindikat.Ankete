using System;
using System.Collections.Generic;
using System.Text;
using SindikatAnkete.Entity;

namespace Sindikat.Ankete.Domain.Models
{
    public class IspuniAnketuDTO
    {
        public int AnketaId { get; set; }
        public virtual ICollection<OdgovorDTO> Odgovor { get; set; }
    }
}
