using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLosowaniaDoOdpowiedzi.Resources.Models
{
    public class Uczen
    {
        public string Imie { get; set; }
        public string Klasa { get; set; }
        public Uczen(string imie, string klasa) 
        {
            Imie = imie;
            Klasa = klasa;
        }
    }
}
