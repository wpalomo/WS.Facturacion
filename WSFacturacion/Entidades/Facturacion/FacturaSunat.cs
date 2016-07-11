using System;
using System.Collections.Generic;

namespace Telectronica.Facturacion
{
    [Serializable]
    public class FacturaSRI
    {
        public byte CodigoEstacion { get; set; }
        public byte NroVia { get; set; }
        public DateTime Fecha { get; set; }
        public int? NumeroEvento { get; set; }
        public string NombreArchivo { get; set; }
        public string XML { get; set; }
        public int Correlativo { get; set; }
        public string Estado { get; set; }

        public string TipoComprobante { get; set; }
        public int Establecimiento { get; set; }
        public int PuntoVenta { get; set; }

        public string RucConcesionario { get; set; }

        public DateTime FechaEmision { get; set; }
        public int SecuencialFactura { get; set; }
    }

    [Serializable]
    public class FacturaSRIL: List<FacturaSRI> {}

    [Serializable]
    public class JornadaSRI
    {
        public string XML { get; set; }
        public string NombreArchivo { get; set; }
        public int Correlativo { get; set; }

        public DateTime Fecha { get; set; }

        public string RucConcesionario { get; set; }
       
        // Punto de venta a utilizar en el CCO
        public string PuntoVentaCCO { get; set; }

    }

    [Serializable]
    public class JornadaSRIL : List<JornadaSRI> {}
}