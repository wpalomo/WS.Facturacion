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
    /// Clase que trae los datos para Reportes de Control de Auditoria
    /// </summary>
    /// ***********************************************************************************************
    public class RptListadoApropiacionesBs
    {
        public static DataSet getDepositosReporte(DateTime jornadaDesde, DateTime jornadaHasta,
                int? zona, int? estacion,
                int? turnoDesde, int? turnoHasta, int? bolsa)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    //DataSet inicial
                    return RptListadoApropiacionesDt.getDepositosReporte(conn, jornadaDesde, jornadaHasta, zona, estacion, turnoDesde, turnoHasta, bolsa);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
