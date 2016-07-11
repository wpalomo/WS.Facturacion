using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class FormatoTicket
    {
        public TipoTicket TipoTicket { get; set; }
        public string CuerpoTicket { get; set; }
        public int NumeroCopias { get; set; }
        public int CodigoTipoTicket
        {
            get { return TipoTicket.Codigo; }
        }

        public string DescrTipoTicket
        {
            get { return TipoTicket.Descripcion; }
        }

    }
    [Serializable]

    public class FormatoTicketL : List<FormatoTicket>
    {
    }
}
