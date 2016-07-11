using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class RptConciliacionVentasBS
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de Conciliacion de ventas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>        
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="nivel">string - nivel de Usuario</param>
        /// <param name="validador">string - id de validador</param>
        /// <param name="agrupacion">string - J: por jornada
        ///                                   P: Por parte</param>
        /// ***********************************************************************************************
        public static DataSet getConciliacionVentas(DateTime jornadaDesde, DateTime jornadaHasta, int? turnoDesde, int? turnoHasta, int? zona, int? estacion,
                                                   String operador, String nivel, String validador, String agrupacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return RptConciliacionVentasDT.getConciliacionVentas(conn, jornadaDesde, jornadaHasta, turnoDesde, turnoHasta, zona, estacion,
                                                                        operador, nivel, validador, agrupacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
