using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{
    public class FacturaXMLItem
    {
        public FacturaXMLItem()
        { 
        }

        public int NroLinDet {get;set;}
        public string NmbItem { get; set;}
        public double MontoItem { get; set;}
    }

    public class FacturaXMLItemL : List<FacturaXMLItem>
    { 
    }
}
