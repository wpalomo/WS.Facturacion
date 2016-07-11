using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using Telectronica.Tesoreria;

namespace Telectronica.Tesoreria
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Control de Auditoria
    /// </summary>
    /// ***********************************************************************************************
    public class RptControlAuditoriaDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de las anomalias
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// ***********************************************************************************************
        public static DataSet getAuditorias(Conexion oConn,
                DateTime? jornadaDesde, DateTime? jornadaHasta,
                int? zona, int? estacion, int? turnoDesde, int? turnoHasta, bool soloModificadas,string anomalias, int? parte, bool conPagosElectronicos)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptAuditoria_ControlAuditoriaDs";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_RptAuditorias_ControlAuditoria";
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;
                oCmd.Parameters.Add("@SoloModificado", SqlDbType.Char, 1).Value = soloModificadas ? "S" : "N";
                oCmd.Parameters.Add("@Fallos", SqlDbType.Char).Value = anomalias;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@conPagosElectro", SqlDbType.Char, 1).Value = conPagosElectronicos ? "S" : "N";

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsPartes, "Partes");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsPartes;
        }


        ///****************************************************************************************************************************
        /// <summary>
        /// Datos para el reporte Anomalias y Cancelaciones
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="jornadaDesde"></param>
        /// <param name="jornadaHasta"></param>
        /// <param name="zona"></param>
        /// <param name="estacion"></param>
        /// <param name="Via"></param>
        /// <param name="turnoDesde"></param>
        /// <param name="turnoHasta"></param>
        /// <param name="soloModificadas"></param>
        /// <param name="anomalias"></param>
        /// <param name="parte"></param>
        /// <param name="Operador"></param>
        /// <param name="Validador"></param>
        /// <returns></returns>
        ///****************************************************************************************************************************
        public static DataSet getAnomaliasCancelaciones(Conexion oConn,
        DateTime? jornadaDesde, DateTime? jornadaHasta,
        int? zona, int? estacion, int? via, int? turnoDesde, int? turnoHasta, bool soloModificadas, string anomalias, int? parte, string Operador, string Validador, bool conPagosElectronicos)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptAuditoria_AnomaliasCancelacionesDs";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_RptAuditorias_InformeAnomalias";
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@via", SqlDbType.TinyInt).Value = via;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;
                oCmd.Parameters.Add("@SoloModificado", SqlDbType.Char, 1).Value = soloModificadas ? "S" : "N";
                oCmd.Parameters.Add("@Fallos", SqlDbType.Char).Value = anomalias;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@Operador", SqlDbType.VarChar,10).Value = Operador;
                oCmd.Parameters.Add("@Validador", SqlDbType.VarChar, 10).Value = Validador;
                oCmd.Parameters.Add("@conPagosElectro", SqlDbType.Char, 1).Value = conPagosElectronicos ? "S" : "N";

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsPartes, "Anomalias");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsPartes;
        }


 
    }
}
