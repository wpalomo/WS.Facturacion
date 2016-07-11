using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region ValePrepago: Clase para entidad de Venta de Vale Prepago

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad de Venta de Vale Prepago
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "VentaVale", IsNullable = false)]

    public class ValePrepago
    {
        // Constructor vacio
        public ValePrepago()
        {
        }


        public ValePrepago(int identity,        Estacion estacion,          DateTime fechaVenta,
                           Cuenta cuenta,       int serieInicial,           int serieFinal,
                           CategoriaManual categoria, FacturaItem itemFactura,    Parte parte, 
                           decimal monto,        string anulada)
        {
            this.Identity = identity;
            this.Estacion = estacion;
            this.FechaVenta = fechaVenta;
            this.Cuenta = cuenta;
            this.SerieInicial = serieInicial;
            this.SerieFinal = serieFinal;
            this.Categoria = categoria;
            this.ItemFactura = itemFactura;
            this.Parte = parte;
            this.Monto = monto;
            this.Anulada = anulada;
        }


        // Identity
        public int Identity { get; set; }

        // Estacion para la cual se vende el vale
        public Estacion Estacion { get; set; }

        // Fecha de la venta
        public DateTime FechaVenta{ get; set; }

        // Cuenta a la cual se le imputa la recarga
        public Cuenta Cuenta { get; set; }

        // Serie inicial del vale
        public int SerieInicial { get; set; }

        // Serie final del vale
        public int SerieFinal { get; set; }

        // Categoria
        public CategoriaManual Categoria { get; set; }

        // Item correspondiente a la factura 
        public FacturaItem ItemFactura { get; set; }

        // Monto de la venta
        public decimal Monto { get; set; }

        // Formatea el monto con el simbolo de moneda ("$")
        public string MontoString
        {
            get
            {
                return Monto.ToString("C");
            }
        }

        // Marca de recarga anulada
        public string Anulada { get; set; }

        // Determina si la entrega esta anulada o no
        public bool esAnulada
        {
            get
            {
                return (Anulada == "S");
            }
        }

        // Parte
        public Parte Parte { get; set; }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ValePrepago
    /// </summary>*********************************************************************************************
    public class ValePrepagoL : List<ValePrepago>
    {
    }

    #endregion
}
