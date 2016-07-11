using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region MOVIMIENTOCAJADENOMINACIONTOTAL: Todos los Items de la liquidacion con el mismo importe de denominacion

    /// <summary>
    /// Clase para todos los itmes de la liquidacion con el mismo valor de denominacion
    /// </summary>
    [Serializable]
    public class MovimientoCajaDenominacionTotal
    {
        public decimal ValorDenominacion { get; set; }

        //Detalle de los items de esta denominacion
        public MovimientoCajaDetalleL Detalle { get; set; }
    }

    /// <summary>
    /// Lista de objetos MovimientoCajaDenominacionTotal.
    /// </summary>
    /// 
    [Serializable]
    public class MovimientoCajaDenominacionTotalL : List<MovimientoCajaDenominacionTotal>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el objeto con el valor de la denominacion
        /// </summary>
        /// <param name="valor">ValorDenominacion - Valor a Buscar
        /// <returns>objeto MovimientoCajaDenominacionTotal que corresponda al valor buscado</returns>
        /// ***********************************************************************************************
        public MovimientoCajaDenominacionTotal FindValor(decimal valor)
        {
            MovimientoCajaDenominacionTotal oMovimiento = null;
            foreach (MovimientoCajaDenominacionTotal oMov in this)
            {
                if (oMov.ValorDenominacion == valor)
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
