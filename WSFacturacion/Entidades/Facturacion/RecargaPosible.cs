using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{

    #region RecargaPosible: Clase de una entidad de recarga posible

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad de Recargas Posibles en via
    /// </summary>*********************************************************************************************
    [Serializable]
    [XmlRootAttribute(ElementName = "RecargaPosible", IsNullable = false)]
    public class RecargaPosible
    {
        //Constructor vacio
        public RecargaPosible() { }

        // Identity
        public string ID { get; set; }
        
        // Estacion
        public Estacion Estacion { get; set; }

        // Tipo de Cuenta 
        public TipoCuenta TipoCuenta { get; set; }

        // Agrupacion 
        public AgrupacionCuenta Agrupacion { get; set; }
        
        // Indica si se puede realizar la recarga en la via
        public bool EnVia { get; set; }
        
        // Monto de la recarga
        public decimal Monto { get; set; }

        // Devolvemos el Monto de la recarga con el formato de moneda
        public string sMonto
        {
            get { return Monto.ToString("C02"); }
        }

        // Para las grillas
        public override string ToString()
        {
            return Convert.ToString(Monto);
        }

        // Codigo del tipo de cuenta
        public int CodigoTipoCuenta
        {
            get
            {
                return TipoCuenta.CodigoTipoCuenta;
            }
        }

        // Descripcion del tipo de cuenta
        public string DescripcionTipoCuenta
        {
            get
            {
                return TipoCuenta.Descripcion;
            }
        }

        // Descripcion de la agrupacion
        public string DescripcionAgrupacion
        {
            get
            {
                return Agrupacion.DescrAgrupacion;
            }
        }

        // Descripcion de la estacion
        public string DescripcionEstacion
        {
            get
            {
                return Estacion.Nombre;
            }
        }
    }

    
    /// *********************************************************************************************<summary>
    /// Lista de objetos RecargaPosible
    /// </summary>*********************************************************************************************
    [Serializable]
    public class RecargaPosibleL : List<RecargaPosible>
    {
        public RecargaPosible FindMinimo()
        {
            RecargaPosible oRec = null;
            if (this.Count > 0)
                oRec = this[0];

            foreach (RecargaPosible item in this)
            {
                if (item.Monto < oRec.Monto)
                    oRec = item;
            }

            return oRec;
        }

        public RecargaPosible FindMaximo()
        {
            RecargaPosible oRec = null;
            if (this.Count > 0)
                oRec = this[0];

            foreach (RecargaPosible item in this)
            {
                if (item.Monto > oRec.Monto)
                    oRec = item;
            }

            return oRec;
        }
    }

    #endregion

}
