using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{

    [Serializable]
    public class BolsaApropiacion
    {
        public Estacion Estacion { get; set; }
        public DateTime Jornada { get; set; }
        public int Turno { get; set; }
        public int NumeroMovimientoCaja { get; set; }
        public string MovimientoDescripcion { get; set; }
        public int? Parte { get; set; }
        public int? Bolsa { get; set; }
        public decimal MontoEquivalente { get; set; }
        public bool Enviada { get; set; }   //Para bindear con el check
    }

    [Serializable]
    public class BolsaApropiacionL : List<BolsaApropiacion>
    {
    }

}
