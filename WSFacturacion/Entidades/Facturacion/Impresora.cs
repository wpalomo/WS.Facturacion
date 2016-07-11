using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region IMPRESORA: Clase para entidad de las Impresoras de Facturacion

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Impresora
    /// </summary>*********************************************************************************************

    [Serializable]

    public class Impresora
    {
        // Constructor Vacio
        public Impresora()
        {
        }


        // Constructor con todos los datos
        public Impresora(Estacion estacion, byte codigo, string puntoVenta, byte cantidadCopias)
        {
            this.Estacion = estacion;
            this.Codigo = codigo;
            this.PuntoVenta = puntoVenta;
            this.CantidadCopias = cantidadCopias;
        }


        // Constructor con todos los datos
        public Impresora(Estacion estacion, byte codigo, string puntoVenta, byte cantidadCopias, byte pCOM, string tipo)
        {
            this.Estacion = estacion;
            this.Codigo = codigo;
            this.PuntoVenta = puntoVenta;
            this.CantidadCopias = cantidadCopias;
            this.PuertoCOM = pCOM;
            this.TipoImpresora = tipo;
        }

        // Estacion en la que se define la impresora
        public Estacion Estacion { get; set; }


        // Codigo de Impresora
        public byte Codigo { get; set; }


        // Punto de Venta
        public string PuntoVenta { get; set; }


        // Cantidad de copias de factura a imprimir 
        public byte CantidadCopias { get; set; }


        // Tipo Autoimpresora or Factura Preimpresa
        public string TipoImpresora { get; set; }


        // Puerto COM al que está conectado
        public Int16 PuertoCOM { get; set; }


        // Url del Web Service que gestiona al lector
        public string UrlServicio { get; set; }

        // Tipo Autoimpresora or Factura Preimpresa
        public string ImpresoraDescription 
        {
            get {
                string Named = "Autoimpresora Serie";
                if (TipoImpresora != null && TipoImpresora.Trim() == "P")
                    Named = "AutoImpresora A4";
                return Named;
            }
        }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Impresora
    /// </summary>*********************************************************************************************
    public class ImpresoraL : List<Impresora>
    {
    }

    #endregion
}
