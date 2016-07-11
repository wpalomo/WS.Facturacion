using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using System.IO;

namespace Telectronica.Peaje
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para auditoría para el reporte de auditoría
    /// </summary>
    /// ***********************************************************************************************
    public class RptAuditoriaBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el log de auditoría
        /// 
        /// </summary>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="codigo">int? - Codigo a buscar</param>
        /// <param name="xmlOperaciones">string - XML con los códigos de Tabaud</param>
        /// ***********************************************************************************************
        public static DataSet getLogAuditoria(DateTime jornadaDesde, DateTime jornadaHasta,
                int? zona, int? estacion, string operador, string codigo, AuditoriaCodigoL oOperaciones)
        {

            DataSet ds = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    ds = RptAuditoriaDt.getLogAuditoria(conn, jornadaDesde, jornadaHasta, zona, estacion, operador, codigo, oOperaciones);

                    //Grabamos auditoria
                    //Auditoria
                    using (Conexion connAud = new Conexion())
                    {
                        //conn.ConectarGSTThenPlaza();
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                        // Ahora tenemos que grabar la auditoria:
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaListado(),
                                                               "C",
                                                               getAuditoriaCodigoRegistro(),
                                                               getAuditoriaDescripcion(jornadaDesde, jornadaHasta, zona, estacion, operador, codigo, oOperaciones)),
                                                               connAud);

                    }

                    return ds;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region AUDITORIA
        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaListado()
        {
            return "AUD";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro()
        {
            return "";
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(DateTime jornadaDesde, DateTime jornadaHasta,
                int? zona, int? estacion, string operador, string codigo, AuditoriaCodigoL oOperaciones)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Desde", jornadaDesde.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Hasta", jornadaHasta.ToShortDateString());
            if( zona != null )
                AuditoriaBs.AppendCampo(sb, "Zona", zona.ToString());
            if (estacion != null)
                AuditoriaBs.AppendCampo(sb, "Estación", estacion.ToString());
            if (operador != null)
                AuditoriaBs.AppendCampo(sb, "Operador", operador);
            if (codigo != null)
                AuditoriaBs.AppendCampo(sb, "Código", codigo);
            if (oOperaciones != null && oOperaciones.Count > 0)
            {
                sb.Append("Operaciones:");
                foreach (AuditoriaCodigo item in oOperaciones)
                {
                    sb.Append(item.Descripcion + ".");
                }
            }
            return sb.ToString();
        }
    #endregion
    }

}
