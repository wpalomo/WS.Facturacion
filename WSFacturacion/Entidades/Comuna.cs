using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class Comuna
    {
        public Comuna()
        { 
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Provincia { get; set; }
        public string Region { get; set; }
    }

    public class ComunaL : List<Comuna>
    { }
}
