using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    [Serializable]
    [XmlRootAttribute(ElementName = "autorizacion", IsNullable = false )]
    public class CambioAutorizacionSRI
    {
        [XmlElement(Order=1)]
        public string codTipoTra { get; set; }
        [XmlElement(Order = 2)]
        public string ruc { get; set; }
        [XmlElement(Order = 3, IsNullable=false)]
        public string autOld { get; set; }
        [XmlElement(Order = 4)]
        public string autNew { get; set; }
        [XmlElement(Order = 5)]
        public string fecha { get; set; }

        public DateTime Inicio;
        public DateTime? InicioAnterior;
        public DateTime Vencimiento;
        public DateTime? VencimientoAnterior;

        [XmlArray(Order = 6, ElementName="detalles")]
        public ListaAutorizacionDetalleL detalles;
    }

    /// <summary>
    /// Lista de objetos ListaAutorizacion.
    /// </summary>
    [Serializable]
    public class CambioAutorizacionSRIL : List<CambioAutorizacionSRI>
    {
    }
}
