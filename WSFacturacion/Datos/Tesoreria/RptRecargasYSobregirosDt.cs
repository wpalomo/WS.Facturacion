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
    public class RptRecargasYSobregirosDt
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
        public static DataSet getRecargasYSobregiros(Conexion oConn,
                DateTime? jornadaDesde, DateTime? jornadaHasta,
                int? zona, int? estacion, int? turnoDesde, int? turnoHasta)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptAuditoria_RecargasYSobregirosDs";

            try
            {


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_RptAuditorias_RecargasYSobregiros";
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;

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
    }
}
