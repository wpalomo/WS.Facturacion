using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using System.Data.SqlClient;

namespace Telectronica.Validacion
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para el Reporte de Observaciones por cajero
    /// </summary>
    /// ***********************************************************************************************
    public class RptObservacionesCajeroDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de Observaciones por cajero
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>  
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="validador">string - id de validador</param>
        /// <param name="anomalia">int? - Codigo de Anomalia</param>
        /// <param name="estadoAnomalia">string - Estado de la anomalia</param>
        /// /// <param name="conObsGralParte">string - Con observacion general del parte</param>
        /// /// <param name="conObsExterna">string - Con observaciones externas</param>
        /// /// <param name="conObsInterna">string - Con observaciones internas</param>
        /// ***********************************************************************************************
        public static DataSet getObservacionesCajero(Conexion oConn, DateTime jornadaDesde, DateTime jornadaHasta, int? zona, int? estacion, String operador, String validador, 
                                                     int? anomalia, String estadoAnomalia, String conObsGralParte, String conObsExterna, String conObsInterna)
        {
             DataSet dsObservaciones = new DataSet();
            dsObservaciones.DataSetName = "RptValidacion_ObservacionesPorCajeroDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_RptValidacion_GetObservacionesCajero";
                oCmd.Parameters.Add("@fDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@fHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@estacion", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@legajo", SqlDbType.VarChar, 10).Value = operador;
                oCmd.Parameters.Add("@idVal", SqlDbType.VarChar, 10).Value = validador;
                oCmd.Parameters.Add("@anomal", SqlDbType.TinyInt).Value = anomalia;
                oCmd.Parameters.Add("@estadoAnomalia", SqlDbType.Char, 1).Value = estadoAnomalia;
                oCmd.Parameters.Add("@obsGralParte", SqlDbType.Char, 1).Value = conObsGralParte;
                oCmd.Parameters.Add("@obsExt", SqlDbType.Char, 1).Value = conObsExterna;
                oCmd.Parameters.Add("@obsInt", SqlDbType.Char, 1).Value = conObsInterna;

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsObservaciones, "Observaciones");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsObservaciones;
        
        }
    }
}
