using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLosowaniaDoOdpowiedzi.Resources.Models
{
    
        public class Klasa
        {
            public string Nazwa { get; set; }
            public List<Uczen> Uczniowie { get; set; }


            public Klasa(string nazwa)
            {
                Nazwa = nazwa;
                Uczniowie = new List<Uczen>();
            }
        }
    }

