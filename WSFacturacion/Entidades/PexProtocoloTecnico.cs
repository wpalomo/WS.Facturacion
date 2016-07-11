using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class PexProtocoloTecnico
    {

        public string Tipo { get; set; }

        public int Administradora { get; set; }

        public DateTime FechaTecnico { get; set; }

        public string CodPais { get; set; }

        public string CodConcesionario { get; set; }

        public int SecuenciaEnviada { get; set; }

        public string Resultado { get; set; }

        public int RegInformados { get; set; }

        public int RegEncontrados { get; set; }

        public decimal TotalInformado { get; set; }

        public decimal TotalEncontrado { get; set; }

    }
}
