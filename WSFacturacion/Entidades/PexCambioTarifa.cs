using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class PexCambioTarifa
    {
        public Estacion Estacion { get; set; }
        public DateTime Fecha { get; set; }
    }

    [Serializable]
    public class PexCambioTarifaL : List<PexCambioTarifa> { }
}
