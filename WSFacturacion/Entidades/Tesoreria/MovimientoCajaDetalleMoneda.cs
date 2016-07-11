using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region MOVIMIENTOCAJADETALLEMONEDA: Todos los Items de la liquidacion con la misma moneda

    /// <summary>
    /// Clase para todos los itmes de la liquidacion con la misma moneda
    /// </summary>
    [Serializable]
    public class MovimientoCajaDetalleMoneda
    {
        public Moneda Moneda { get; set; }

        public decimal Cotizacion { get; set; } 
        //Detalle de los items de esta denominacion
        public MovimientoCajaDetalleL Detalle { get; set; }
    }

    /// <summary>
    /// Lista de objetos MovimientoCajaDetalleMoneda.
    /// </summary>
    /// 
    [Serializable]
    public class MovimientoCajaDetalleMonedaL : List<MovimientoCajaDetalleMoneda>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el objeto con el valor de la denominacion
        /// </summary>
        /// <param name="valor">ValorDenominacion - Valor a Buscar
        /// <returns>objeto MovimientoCajaDenominacionTotal que corresponda al valor buscado</returns>
        /// ***********************************************************************************************
        public MovimientoCajaDetalleMoneda FindValor(Moneda moneda)
        {
            MovimientoCajaDetalleMoneda oMovimiento = null;
            foreach (MovimientoCajaDetalleMoneda oMov in this)
            {
                if (oMov.Moneda.Codigo == moneda.Codigo)
                {
                    oMovimiento = oMov;
                    break;
                }
            }
            return oMovimiento;
        }
    }

    #endregion
}
