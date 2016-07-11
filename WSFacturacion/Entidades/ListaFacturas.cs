using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;

namespace Telectronica.Peaje
{
    [Serializable]
    public class ListaFacturas
    {
        public string Identificado;
        public string RazonSocialConcesionario;
        public string RucConcesionario;
        public string DireccionMatriz;
        public string DireccionEstablecimiento;
        public string AutorizacionSRI;
        public DateTime VencimientoSRI;
        public DateTime InicioSRI;
        public string ContribuyenteEspecial;
        public string TipoComprobante;
        public int Establecimiento;
        public int PuntoVenta;
        public int SecuencialFactura;
        public DateTime FechaEmision;
        public string RucCliente;
        public string NombreCliente;
        public decimal MontoNeto;
        public decimal MontoIVA;
        public decimal MontoTotal;
        public bool Anulado;

        public ListaFacturasDetalleL detalles;
    }



    /// <summary>
    /// Lista de objetos ListaFacturas.
    /// </summary>
    [Serializable]
    public class ListaFacturasL : List<ListaFacturas>
    {
    }
}
