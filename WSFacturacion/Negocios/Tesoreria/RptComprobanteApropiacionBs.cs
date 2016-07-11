using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Tesoreria
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Detalle de una Apropiacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptComprobanteApropiacionBs
    {



        public static DataSet getApropiacionCompleta(Apropiacion oApropiacion)
        {
            DataSet dsApropiacion = null;

            try
            {
                using (Conexion conn = new Conexion())
                {

                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    dsApropiacion = RptComprobanteApropiacionDT.getBolsas(conn, oApropiacion.Estacion.Numero, oApropiacion.NumeroApropiacion);
                    RptComprobanteApropiacionDT.getDetalle(conn, dsApropiacion, oApropiacion.Estacion.Numero, oApropiacion.NumeroApropiacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsApropiacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet 
        /// </summary>
        /// <param name="apr">int - Numero de Apropiacion</param>
        /// <param name="estacion">int - Codigo de Estacion</param>
        /// ***********************************************************************************************
        public static void getDetalle(int apr, int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    //DataSet dsApropiacion = RptComprobanteApropiacionDT.getDetalle(conn, estacion, apr);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;

        }
  
    
    }
}
