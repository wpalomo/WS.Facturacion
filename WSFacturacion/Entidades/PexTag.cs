using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class PexTag
    {
        //public string TipoArchivo { get; set; }

        public int Secuencia { get; set; }

        //public DateTime FechaGeneracion { get; set; }

        //public byte TotalRegistros { get; set; }
        
        public string Tipo { get; set; }

        public int Administradora { get; set; }

        public string PaisEmisor { get; set; }

        public string EmisorTag { get; set; }

        public string IdEmisorTag { get; set; }

        public string NumeroTag { get; set; }

        public string Placa { get; set; }

        public byte Categoria { get; set; }

        public string Operacion { get; set; }

        public byte DiaPago { get; set; }

        public string MedioPago { get; set; }

        public string FormaPago { get; set; }

        public string TipoDispositivo { get; set; }




    }
}
