using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para el Reporte de Observaciones por cajero
    /// </summary>
    /// ***********************************************************************************************
   

    public class RptFallosPeajistaBS
    {

        /// ***********************************************************************************************
        /// <summary>
        /// lista de fallos por cajero
        /// </summary>             
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>  
        /// <param name="turnoDesde">int? - turno Desde</param>
        /// <param name="turnoHasta">int? - turno Hasta</param>  
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="nivel">string - nivel (Cajero, Supervisor)</param>
        /// <param name="parte">int? - numero de parte</param>
        /// <param name="tipoResultado">string - tipo de resultado (Positivo, Negativo, neutral)</param>
        /// <param name="validador">string - id de validador</param>  
        /// ***********************************************************************************************

        public static DataSet getObservacionesCajero(DateTime jornadaDesde, DateTime jornadaHasta, int? turnoDesde, int? turnoHasta, int? zona, int? estacion, String operador,
                                                     String nivel, int? parte, decimal? res1Desde, decimal? res1Hasta, decimal? res2Desde, decimal? res2Hasta, String tipoResultado, String validador)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return RptFallosPeajistaDt.getObservacionesCajero(conn, jornadaDesde, jornadaHasta, turnoDesde, turnoHasta, zona, estacion, operador,
                                                     nivel, parte, res1Desde, res1Hasta, res2Desde, res2Hasta, tipoResultado, validador);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
