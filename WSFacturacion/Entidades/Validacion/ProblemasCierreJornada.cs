using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Validacion
{
    public class ProblemasCierreJornada
    {
        public int estacion { get; set; }
        public int? parte { get; set; }
        public string problema { get; set; }
        public string peajista { get; set; }
        public string tipo { get; set; }
        public string datos { get; set; }

        public ProblemasCierreJornada()
        {

        }

        public ProblemasCierreJornada(string problemaCierre, string datosCierre)
        {
            this.problema = problemaCierre;
            this.datos = datosCierre;
        }

    }
    public class ProblemasCierreJornadaL:List<ProblemasCierreJornada>
    {
    }
}
