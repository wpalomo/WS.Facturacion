using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para el Reporte de Conciliacion de Ventas
    /// </summary>
    /// ***********************************************************************************************
    public class RptConciliacionVentasDT
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
        public static DataSet getConciliacionVentas(Conexion oConn, DateTime jornadaDesde, DateTime jornadaHasta, int? turnoDesde, int? turnoHasta, int? zona, int? estacion,
                                                    String operador, String nivel, String validador, String agrupacion)
        {
            DataSet dsConciliacion = new DataSet();
            dsConciliacion.DataSetName = "RptValidacion_ConciliacionVentasDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_RptValidacion_GetConciliacionVentas";
                oCmd.Parameters.Add("@fDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@fHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@turnoIni", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@turnoFin", SqlDbType.TinyInt).Value = turnoHasta;
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@estacion", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@idOper", SqlDbType.VarChar, 10).Value = operador;
                oCmd.Parameters.Add("@nivel", SqlDbType.VarChar, 15).Value = nivel;
                oCmd.Parameters.Add("@idVal", SqlDbType.VarChar, 10).Value = validador;
                oCmd.Parameters.Add("@agrupacion", SqlDbType.Char, 1).Value = agrupacion;

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsConciliacion, "Conciliacion");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsConciliacion;
        }
    }
}
