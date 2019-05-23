using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Ankete.Domain.Models
{
    public class AnketaMenuDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public DateTime VrijemeKreiranja { get; set; }
        public string Opis { get; set; }
    }
}
