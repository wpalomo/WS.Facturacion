using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class OSAsCambioTarifa
    {
        public Estacion Estacion { get; set; }
        public DateTime Fecha { get; set; }
    }

    [Serializable]
    public class OSAsCambioTarifaL : List<OSAsCambioTarifa> { }
}
