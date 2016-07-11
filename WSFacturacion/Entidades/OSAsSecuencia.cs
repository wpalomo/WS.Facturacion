using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class OSAsSecuencia
    {
        public string Directorio { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int Registros { get; set; }
        public decimal MontoTotal { get; set; }
        public int? CantFotos { get; set; }
        public int NroSecuencia { get; set; }
        public string TipoArchivo { get; set; }
        public int AdministradoraTag { get; set; }
        public string AdministradoraTagDescr { get; set; }
        public Estacion Estacion { get; set; }
        public DateTime Jornada { get; set; }


        public int SecuenciaTRN { get; set; }
        public int SecuenciaTAF { get; set; }
        public int SecuenciaTAG { get; set; }
        public int SecuenciaNEL { get; set; }
        public string Problema { get; set; }

    }

    [Serializable]
    public class OSAsSecuenciaL : List<OSAsSecuencia>
    {
    }

}
