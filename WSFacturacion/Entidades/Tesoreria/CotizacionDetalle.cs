using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Tesoreria
{
    #region COTIZACIONDETALLE: Clase para entidad de detalle de las Cotizaciones 

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad CotizacionDetalle
    /// </summary>*********************************************************************************************

    [Serializable]

    public class CotizacionDetalle
    {
        // Constructor Vacio
        public CotizacionDetalle()
        {
        }


        // En el Constructor asignamos los valores
        public CotizacionDetalle(Moneda moneda, decimal valor)
        {
            this.Moneda = moneda;
            this.ValorCotizacion = valor;
        }


        // Moneda a la que pertecene el valor de la cotizacion
        public Moneda Moneda { get; set; }

        // Valor de la cotizacion
        public decimal ValorCotizacion { get; set; }

        // Valor de la cotizacion en formato para grilla
        public string sValorCotizacion
        {
            get
            {
                return ValorCotizacion.ToString("F02");
            }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos CotizacionDetalle
    /// </summary>*********************************************************************************************
    public class CotizacionDetalleL : List<CotizacionDetalle>
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una determinada CotizacionDetalle localizada mediante los parametros
        /// </summary>
        /// <param name="moneda">int16 - Moneda de la que se quiere saber la cotizacion</param>
        /// <returns>objeto CotizacionDetalle que corresponda a la moneda buscada</returns>
        /// ***********************************************************************************************
        public CotizacionDetalle FindCotizacionDetalle(Int16 moneda)
        {
            CotizacionDetalle oCotizacionDetalle  = null;

            foreach (CotizacionDetalle oCot in this)
            {
                if (moneda == oCot.Moneda.Codigo)
                {
                    oCotizacionDetalle= oCot;
                    break;
                }
            }

            return oCotizacionDetalle;
        }

    }

    #endregion
}
