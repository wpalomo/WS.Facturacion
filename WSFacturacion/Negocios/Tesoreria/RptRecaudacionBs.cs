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
    /// Clase que trae los datos para Reportes de Recaudacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptRecaudacionBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la lista de partes con su recaudacion
        /// </summary>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="turno">int? - turno</param>
        /// <param name="tipoParte">string - N sin liquidar
        ///                                  S liquidado
        ///                                  V validado
        ///                                  null todos</param>
        /// ***********************************************************************************************
        public static DataSet getPartes(DateTime jornadaDesde, DateTime jornadaHasta, int? zona, int? estacion, string operador, int? turno, string tipoParte, string sModoParte)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptRecaudacionDt.getPartes(conn, jornadaDesde, jornadaHasta, zona, estacion, operador, turno, tipoParte, sModoParte);
            }
        }
        /// <summary>
        /// Devuelve un dataSet para el reporte de MDA
        /// </summary>
        /// <param name="jornadaDesde">Fecha desde</param>
        /// <param name="jornadaHasta">Fecha Hasta</param>
        /// <param name="estacion">estacion, 0 para todas</param>
        /// <param name="turnoDesde">Turno inicial</param>
        /// <param name="turnoHasta">Turno final</param>
        /// <param name="tipoParte">string - N sin liquidar
        ///                                  S liquidado
        ///                                  V validado
        ///                                  null todo</param>
        /// <param name="sModoParte">Periodo, dia o mes.</param>
        /// <returns></returns>
        public static DataSet getMDA(DateTime jornadaDesde, DateTime jornadaHasta, int? estacion, int? turnoDesde, int? turnoHasta, string tipoParte, string sModoParte,bool xEstacion)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidado(ConexionBs.getGSToEstacion(), false);
                return RptRecaudacionDt.getMDA(conn,jornadaDesde, jornadaHasta, estacion, turnoDesde, turnoHasta, tipoParte, sModoParte, xEstacion);
            }
        }



		/// <summary>
		/// Devuelve un dataSet para el reporte de MDA
		/// </summary>
		/// <param name="jornadaDesde">Fecha desde</param>
		/// <param name="jornadaHasta">Fecha Hasta</param>
		/// <param name="estacion">estacion, 0 para todas</param>
		/// <param name="turnoDesde">Turno inicial</param>
		/// <param name="turnoHasta">Turno final</param>
		/// <param name="tipoParte">string - N sin liquidar
		///                                  S liquidado
		///                                  V validado
		///                                  null todo</param>
		/// <param name="sModoParte">Periodo, dia o mes.</param>
		/// <returns></returns>
		public static DataSet getMDAContable(DateTime jornadaDesde, DateTime jornadaHasta, int? estacion,  string tipoParte,bool xEstacion)
		{
			using (Conexion conn = new Conexion())
			{
				//sin transaccion
				conn.ConectarConsolidado(ConexionBs.getGSToEstacion(), false);
				return RptRecaudacionDt.getMDAContable(conn, jornadaDesde, jornadaHasta, estacion, tipoParte, xEstacion);
			}
		}


    }
}
