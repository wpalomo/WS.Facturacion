using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class RptObservacionesCajeroBs
    {
        
        /// ***********************************************************************************************
        /// <summary>
        /// lista de Observaciones por cajero
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>  
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="validador">string - id de validador</param>
        /// <param name="anomalia">int? - Codigo de Anomalia</param>
        /// <param name="estadoAnomalia">string - Estado de la anomalia</param>
        /// /// <param name="conObsGralParte">string - Con observacion general del parte</param>
        /// /// <param name="conObsExterna">string - Con observaciones externas</param>
        /// /// <param name="conObsInterna">string - Con observaciones internas</param>
        /// ***********************************************************************************************
        public static DataSet getObservacionesCajero(DateTime jornadaDesde, DateTime jornadaHasta, int? zona, int? estacion, String operador, String validador,
                                                     int? anomalia, String estadoAnomalia, String conObsGralParte, String conObsExterna, String conObsInterna)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return RptObservacionesCajeroDt.getObservacionesCajero(conn, jornadaDesde, jornadaHasta, zona, estacion, operador, validador,
                                                     anomalia, estadoAnomalia, conObsGralParte, conObsExterna, conObsInterna);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
