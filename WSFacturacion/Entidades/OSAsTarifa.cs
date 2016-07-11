using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class OSAsTarifa
    {
        public Estacion Estacion { get; set; }
        public string TipoEstablecimiento { get; set; }
        public string CodEstablecimiento { get; set; }
        public string CodPlaza { get; set; }
        public CategoriaManual Categoria { get; set; }
        public string DiaSemana { get; set; }
        public decimal Valor { get; set; }
        public string TipoInformacion { get; set; }
        public DateTime FechaInicio { get; set; }
    }

    [Serializable]
    public class OSAsTarifaL : List<OSAsTarifa> { }
}
