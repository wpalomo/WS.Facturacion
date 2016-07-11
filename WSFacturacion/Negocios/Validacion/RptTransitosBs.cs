using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class RptTransitosBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de Transitos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>        
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="parte">int? - Parte</param>  
        /// <param name="via">int? - Via</param>  
        /// ***********************************************************************************************
        public static DataSet getTransitos(DateTime jornadaDesde, DateTime jornadaHasta, int? estacion,
            int? parte, int? via)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return RptTransitosDT.getTransitos(conn, jornadaDesde, jornadaHasta, estacion, parte, via);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
