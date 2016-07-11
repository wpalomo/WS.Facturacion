using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{ [Serializable]
    public class TipoTicket
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
    }
    [Serializable]

    public class TipoTicketL:List<TipoTicket>
    {
    }
}
