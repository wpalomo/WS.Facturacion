using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    [Serializable]
    public class ApropiacionDetalle
    {
        // Numero de movimiento de Mocaja. 
        public int MovimientoCaja { get; set; }
    }

    [Serializable]
    public class ApropiacionDetalleL : List<ApropiacionDetalle>
    {
    }
}
