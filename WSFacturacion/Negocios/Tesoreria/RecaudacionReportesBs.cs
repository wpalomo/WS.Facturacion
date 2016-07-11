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
    public class RecaudacionReportesBs
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
        public static DataSet getPartes(DateTime jornadaDesde, DateTime jornadaHasta,
                int? zona, int? estacion, string operador, int? turno, string tipoParte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RecaudacionReportesDt.getPartes(conn, jornadaDesde, jornadaHasta, zona, estacion, operador, turno, tipoParte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
