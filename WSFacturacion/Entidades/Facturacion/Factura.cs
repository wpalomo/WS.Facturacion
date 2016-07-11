using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;


namespace Telectronica.Facturacion
{
    #region FACTURA: Clase para entidad de Facturas generadas

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Factura
    /// </summary>*********************************************************************************************

    [Serializable]

    public class Factura
    {
        // Constructor vacio
        public Factura()
        {
        }

        
        // Identity
        public int Identity { get; set; }

        // Estacion donde se genero
        public Estacion Estacion { get; set; }

        // Tipo de Factura
        public string TipoFactura { get; set; }
        public string TipoFacturaDescr { get; set; }

        //Serie de la factura
        public string Serie { get; set; }

        // Numero de factura
        public int NumeroFactura { get; set; }

        // Serie Preimpresa
        public string SeriePreimpresa { get; set; }

        // Numero de factura preimpresa
        public int NumeroFacturaPreimpresa { get; set; }

        // Punto de Venta
        public string PuntoVenta { get; set; }

        // Parte en el que se genero la factura
        public Parte Parte { get; set; }

        // Fecha de generacion
        public DateTime FechaGeneracion { get; set; }

        // Cliente al que se le facturo
        public Cliente Cliente { get; set; }
     
        // Marca que indica si esta o no anulada la factura
        public bool Anulada { get; set; }

        // Observacion
        public string Observacion { get; set; }

        // Monto de la factura
        public decimal MontoTotal { get; set; }

        // Monto Neto (sin IVA)
        public decimal MontoNeto { get; set; }

        // Monto de IVA
        public decimal MontoIVA { get; set; }

        // Monto de Retencion en la fuente
        public decimal MontoRetencion { get; set; }

        // Monto de Retencion en la fuente (Bienes)
        public decimal MontoRetencionBienes { get; set; }

        // Monto A Cobrar
        public decimal MontoACobrar { get; set; }

        // Si es una factura para reemplazo de ticket
        public bool FacturaReemplazoTicket { get; set; }

        // Indica si la factura tiene asociada una nota de credito
        public bool NotaCredito { get; set; }

        // Indica si la factura se emitió impaga
        public bool CobroACuenta { get; set; }

        // Indica si la factura esta paga (por registrarse directamente como paga o haberse hecho el cobro a cuenta)
        public bool Cobrada { get; set; }

        //Items de la Factura
        /****
         Dmitriy: 14/04/2011
         **/
        public FacturaItemL Items { get; set; }

        public string NumeroAutorizacionSRI { get; set; }

        public DateTime FechaInicioSRI { get; set; }

        public DateTime FechaCaducidadSRI { get; set; }

        public int Establecimiento { get; set; }

        public string NombreConcesionario { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Factura
    /// </summary>*********************************************************************************************
    public class FacturaL : List<Factura>
    {
    }

    #endregion
}
