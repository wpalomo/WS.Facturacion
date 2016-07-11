using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Telectronica.Facturacion
{   [Serializable]
    public class FacturaXML
    {
        public FacturaXML()
        { 
        }
        
        //*Estructura del XML expresada con comentarios*
        public int Ident { get; set;}
        //<Documento
        //<Encabezado>
        //<IdDoc>
        public int TipoDTE { get; set; }
        public int Folio { get; set; }
        public DateTime FchEmis { get; set; }
        public DateTime FchVenc { get; set; }
        //</IdDoc>
        //<Emisor>
        public Int64 RUTEmisor { get; set; }
        public string RznSoc { get; set; }
        public int GiroEmis { get; set; }
        public string Acteco { get; set; }
        public string DirOrigen { get; set; }
        public string CmnaOrigen { get; set; }
        public string CiudadOrigen { get; set; }
        //</Emisor>
        //<Receptor>
        public Int64 RUTRecep { get; set; }
        public string RznSocRecep { get; set; }
        public double GiroRecep { get; set; }
        public string DirRecep { get; set; }
        public string CmnaRecep { get; set; }
        public string CiudadRecep { get; set; }
        //</Receptor>
        //<Totales>
        public double MntExe { get; set; }
        public double MntTotal { get; set; }
        public double SaldoAnteriorInt { get; set; }
        public double VlrPagar { get; set; }
        //</Totales>
        //</encabezado>
        
        //<Detalle>
        public FacturaXMLItemL items;
        //</Detalles>
        
        //<Adjuntos>
        public string TpoPago { get; set; }
        public double SaldoAnteriorSInt { get; set;}
        public double Intereses { get; set;}
        public string MailReceptor { get; set;}
        public string MailEmisor { get; set;}
        public string Subject { get; set; }
        public string Impresora { get; set; }
        public int Copias { get; set; }
        //</Adjuntos>
        
        //</Documento>
    }
    [Serializable]
    public class FacturaXMLL : List<FacturaXML>
    { 
    }
}
