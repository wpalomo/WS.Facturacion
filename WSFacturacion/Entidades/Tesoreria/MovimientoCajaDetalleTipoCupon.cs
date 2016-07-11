using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region MOVIMIENTOCAJADETALLETIPOCUPON: Todos los Items de la liquidacion de cupones con el mismo tipo de cupon

    /// <summary>
    /// Clase para todos los itmes de la liquidacion con el mismo tipo de cupon 
    /// </summary>
    [Serializable]
    public class MovimientoCajaDetalleTipoCupon
    {
        /// <summary>
        /// Cabecera TipoCupon
        /// </summary>
        public TipoCupon tipoCupon { get; set; }

        /// <summary>
        /// Detalle de los items de esta denominacion
        /// </summary>
        public MovimientoCajaDetalleCuponL Detalle { get; set; }

        /// <summary>
        /// Obtiene o establece el total por tipo de vale
        /// </summary>
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Lista de objetos MovimientoCajaDetalleTipoCupon.
    /// </summary>
    [Serializable]
    public class MovimientoCajaDetalleTipoCuponL : List<MovimientoCajaDetalleTipoCupon>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el objeto con este tipo de cupon
        /// </summary>
        /// <param name="valor">TipoCupon - Valor a Buscar
        /// <returns>objeto MovimientoCajaDetalleTipoCupon que corresponda al valor buscado</returns>
        /// ***********************************************************************************************
        public MovimientoCajaDetalleTipoCupon FindValor(TipoCupon tipoCupon)
        {
            MovimientoCajaDetalleTipoCupon oMovimiento = null;

            foreach (MovimientoCajaDetalleTipoCupon oMov in this)
            {
                if (oMov.tipoCupon.Codigo == tipoCupon.Codigo)
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
