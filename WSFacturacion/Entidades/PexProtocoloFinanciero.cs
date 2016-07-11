using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class PexProtocoloFinanciero
    {
        public int Secuencia { get; set; }

        public int SecuenciaEnviada { get; set; }

        public int Administradora { get; set; }

        public int NumeroRegistro { get; set; }

        public string Tipo { get; set; }

        public string PaisEmisor { get; set; }

        public string EmisorTag { get; set; }

        public string NumeroTag { get; set; }

        public DateTime FechaRegEnviado { get; set; }        

        public string CodigoPlaza { get; set; }

        public string CodigoPista { get; set; }

        public decimal ValorPasada { get; set; }

        public string CodRetorno { get; set; }

        public string Placa { get; set; }

        public decimal ValorPago { get; set; }

        public DateTime FechaPago { get; set; }

    }
}
