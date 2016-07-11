using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Tesoreria;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    [Serializable]
    public class TicketManual
    {
        public string PuntoVenta { get; set; }
        public int Ticket { get; set; }
        public Estacion Estacion { get; set; }
        public Via Via { get; set; }
        public Parte Parte { get; set; }
        public Usuario Cajero { get; set; }
        public DateTime Fecha { get; set; }
        public Usuario Validador { get; set; }
        public int CodigoRegistro { get; set; }
    }

    [Serializable]
    public class TicketManualL : List<TicketManual>
    {
    }
}
