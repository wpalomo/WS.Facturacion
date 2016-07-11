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
    public class RptRecargasYSobregirosBs
    {
        public static DataSet getRecargasYSobregiros(DateTime jornadaDesde, DateTime jornadaHasta,
                int? zona, int? estacion,
                int? turnoDesde, int? turnoHasta)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    //DataSet inicial
                    return RptRecargasYSobregirosDt.getRecargasYSobregiros(conn, jornadaDesde, jornadaHasta,zona, estacion, turnoDesde, turnoHasta);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
