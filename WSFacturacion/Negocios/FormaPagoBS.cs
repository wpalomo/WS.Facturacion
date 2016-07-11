using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Validacion;

namespace Telectronica.Peaje
{
    public class FormaPagoBS
    {
        #region TIPOPAGO: Clase de Negocios de tipos de pago
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los tipos de pago definidos.
        /// </summary>
        /// <returns>Lista de Tipo de Pago</returns>
        /// ***********************************************************************************************
        public static TipoPagoL getTiposAutomaticos()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return FormaPagoDt.getTiposAutomaticos(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las formas de pago.
        /// </summary>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static FormaPagoL getFormasPago()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return FormaPagoDt.getFormasPago(conn,null,null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las formas de pago.
        /// </summary>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static CausaSupervisionL CodigoAceptacionSupRemota()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return FormaPagoDt.getCodigosAceptacionSupervicionRemota(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
