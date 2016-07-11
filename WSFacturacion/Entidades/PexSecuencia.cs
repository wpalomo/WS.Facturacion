using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class PexSecuencia
    {
        public string Directorio { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int Registros { get; set; }
        public decimal MontoTotal { get; set; }
        public int? CantFotos { get; set; }
        public int NroSecuencia { get; set; }
        public string TipoArchivo { get; set; }
        public int AdministradoraTag { get; set; }
        public Estacion Estacion { get; set; }
        public DateTime Jornada { get; set; }
    }

    [Serializable]
    public class PexSecuenciaL : List<PexSecuencia>
    {
    }

}
