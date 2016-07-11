using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;

namespace Telectronica.Facturacion
{
    #region NOTACREDITO: Clase para entidad de Notas de Credito aplicadas a las facturas

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad NotaCredito
    /// </summary>*********************************************************************************************

    [Serializable]

    public class NotaCredito
    {
        // Constructor vacio
        public NotaCredito()
        {
        }


        // Identity
        public int Identity { get; set; }

        // Estacion donde se genero
        public Estacion Estacion { get; set; }

        //Datos de la Factura
        public Factura Factura { get; set; }

        //Serie de la Nota de Credito
        public string Serie { get; set; }

        // Numero de Nota de Credito
        public int NumeroNC { get; set; }

        // Serie Preimpresa
        public string SeriePreimpresa { get; set; }

        // Numero de NC preimpresa
        public int NumeroNCPreimpresa { get; set; }

        // Punto de Venta de la NC
        public string PuntoVenta { get; set; }

        // Parte en el que se genero la NC
        public Parte Parte { get; set; }

        // Fecha de generacion
        public DateTime FechaGeneracion { get; set; }

        // Cliente al que se le hace la NC
        public Cliente Cliente { get; set; }

        // Marca que indica si esta o no anulada la NC
        public bool Anulada { get; set; }

        // Observacion de la NC
        public string Observacion { get; set; }

        // Monto de la NC
        public decimal MontoTotal { get; set; }

        // Monto Neto (sin IVA)
        public decimal MontoNeto { get; set; }

        // Monto de IVA
        public decimal MontoIVA { get; set; }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos NotaCredito
    /// </summary>*********************************************************************************************
    public class NotaCreditoL : List<NotaCredito>
    {
    }

    #endregion
}
