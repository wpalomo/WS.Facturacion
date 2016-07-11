using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    [Serializable]
    [XmlRoot(ElementName = "detalle")]
    public class ListaAutorizacionDetalle
    {
        public int codDoc;
        public string estab;
        public string ptoEmi;
        [XmlElement(IsNullable=false)]
        public string finOld;
        [XmlElement(IsNullable = false)]
        public string iniNew;
        [XmlElement(IsNullable = false)]
        public string finNew;

    }
    /// <summary>
    /// Lista de objetos ListaAutorizacionDetalle.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "detalles")]
    public class ListaAutorizacionDetalleL : List<ListaAutorizacionDetalle>
    {
    }
}
