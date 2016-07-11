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
    /// Clase que trae los datos para Reportes de Recaudacion
    /// </summary>
    /// ***********************************************************************************************
    public class RecaudacionReportesDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la lista de partes con su recaudacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
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
        public static DataSet getPartes(Conexion oConn, DateTime jornadaDesde, DateTime jornadaHasta,
                int? zona, int? estacion, string operador, int? turno, string tipoParte)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RecaudacionPartesDS";
            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.Peaj0309";
                oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = operador;
                oCmd.Parameters.Add("@turno", SqlDbType.TinyInt).Value = turno;
                oCmd.Parameters.Add("@TipoParte", SqlDbType.Char, 1).Value = tipoParte;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsPartes,"Partes");

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
