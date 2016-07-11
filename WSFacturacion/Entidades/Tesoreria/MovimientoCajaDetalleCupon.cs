using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region MOVIMIENTOCAJADETALLECUPON: Item de la liquidacion de cupones
    
    /// <summary>
    /// Clase para cada item de una liquidacion de cupones
    /// </summary>
    [Serializable]
    public class MovimientoCajaDetalleCupon
    {
        public int cantidad { get; set; }
        public TipoCupon tipoCupon { get; set; }
        public decimal montoUnitario { get; set; }
        public decimal montoTotal { get; set; }
        public CategoriaManual categoria { get; set; }
    }

    /// <summary>
    /// Lista de objetos MovimientoCajaDetalleCupon.
    /// </summary>
    [Serializable]
    public class MovimientoCajaDetalleCuponL : List<MovimientoCajaDetalleCupon>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve agrupados por el tipo de cupon
        /// </summary>
        /// <returns>MovimientoCajaDetalleTipoCuponL</returns>
        /// ***********************************************************************************************
        public MovimientoCajaDetalleTipoCuponL getPorTipoCupon()
        {
            MovimientoCajaDetalleTipoCuponL oMovimientos = new MovimientoCajaDetalleTipoCuponL();

            foreach (MovimientoCajaDetalleCupon item in this)
            {
                MovimientoCajaDetalleTipoCupon oMov = oMovimientos.FindValor(item.tipoCupon);
                if (oMov == null)
                {
                    oMov = new MovimientoCajaDetalleTipoCupon();
                    oMov.tipoCupon = item.tipoCupon;
                    oMov.Detalle = new MovimientoCajaDetalleCuponL();

                    oMovimientos.Add(oMov);
                }
                oMov.Detalle.Add(item);
            }

            return oMovimientos;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de categorias usadas (y las activas)
        /// Si se dio de baja una moneda la seguimos teniendo para liquidaciones viejas
        /// </summary>
        /// <returns>CategoriaManualL</returns>
        /// ***********************************************************************************************
        public CategoriaManualL getCategorias()
        {
            CategoriaManualL oCategorias = new CategoriaManualL();

            foreach (MovimientoCajaDetalleCupon item in this)
            {
                CategoriaManual oCat = oCategorias.FindCategoria(item.categoria.Categoria);
                if (oCat == null)
                {
                    //TODO insertar ordenado
                    oCategorias.Add(item.categoria);
                }
            }

            return oCategorias;
        }
    }

    #endregion
}
