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
    public class RptControlAuditoriaBs
    {
        public static DataSet getAuditorias(DateTime jornadaDesde, DateTime jornadaHasta,
                int? zona, int? estacion,
                int? turnoDesde, int? turnoHasta, bool soloModificadas, string anomalias,int? parte, bool conPagosElectronicos)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    //DataSet inicial

                    //if (parte != null)
                    //{
                    //    return RptControlAuditoriaDt.getAuditorias(conn, null, null, zona, estacion, turnoDesde, turnoHasta, soloModificadas, anomalias, parte, conPagosElectronicos);
                    //}
                    //else
                    {

                        return RptControlAuditoriaDt.getAuditorias(conn, jornadaDesde, jornadaHasta, zona, estacion, turnoDesde, turnoHasta, soloModificadas, anomalias, parte, conPagosElectronicos);
                    }
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene la informacion para el informe de Anomalias y Cancelaciones
        /// </summary>
        /// <param name="jornadaDesde">Jornada Inicio</param>
        /// <param name="jornadaHasta">Jornada Fin</param>
        /// <param name="zona">Zona</param>
        /// <param name="estacion">Estacion</param>
        /// <param name="via">Via</param>
        /// <param name="turnoDesde">Turno Desde</param>
        /// <param name="turnoHasta">Turno Hasta</param>
        /// <param name="soloModificadas">Solo Anomalias modificadas</param>
        /// <param name="anomalias">Tipos de Anomalias</param>
        /// <param name="parte">Parte</param>
        /// <param name="Operador">Operados - Cajero</param>
        /// <param name="Validador">Validador</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getAnomaliasCancelaciones(DateTime? jornadaDesde, DateTime? jornadaHasta,
                                                        int? zona, int? estacion, int? via, int? turnoDesde, int? turnoHasta, 
                                                        bool soloModificadas, string anomalias, int? parte, string Operador, string Validador,bool conPagosElectronicos)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RptControlAuditoriaDt.getAnomaliasCancelaciones(conn, jornadaDesde, jornadaHasta, zona, estacion, via, turnoDesde, turnoHasta, soloModificadas, anomalias, parte, Operador, Validador, conPagosElectronicos);
                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }


}
