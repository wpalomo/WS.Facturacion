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
    /// Clase que trae los datos para el Reporte de Observaciones por cajero
    /// </summary>
    /// ***********************************************************************************************
    
    public class RptFallosPeajistaDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de fallos por cajero
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>  
        /// <param name="turnoDesde">int? - turno Desde</param>
        /// <param name="turnoHasta">int? - turno Hasta</param>  
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="nivel">string - nivel (Cajero, Supervisor)</param>
        /// <param name="parte">int? - numero de parte</param>
        /// <param name="tipoResultado">string - tipo de resultado (Positivo, Negativo, neutral)</param>
        /// <param name="validador">string - id de validador</param>  
        /// ***********************************************************************************************
        
        public static DataSet getObservacionesCajero(Conexion oConn, DateTime jornadaDesde, DateTime jornadaHasta, int? turnoDesde, int? turnoHasta, int? zona, int? estacion, String operador, 
                                                     String nivel, int? parte, decimal? res1Desde, decimal? res1Hasta, decimal? res2Desde, decimal? res2Hasta, String tipoResultado, String validador)
        {
            DataSet dsFallos = new DataSet();
            dsFallos.DataSetName = "RptValidacion_FallosPeajistaDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_RptValidacion_GetFallosPeajistas";
                oCmd.Parameters.Add("@fejordes", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@fejorhas", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@turdesde", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@turhasta", SqlDbType.TinyInt).Value = turnoHasta;
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@legajo", SqlDbType.VarChar, 10).Value = operador;
                oCmd.Parameters.Add("@nivel", SqlDbType.VarChar, 15).Value = nivel;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@res1Desde", SqlDbType.Money).Value = res1Desde;
                oCmd.Parameters.Add("@res1Hasta", SqlDbType.Money).Value = res1Hasta;
                oCmd.Parameters.Add("@res2Desde", SqlDbType.Money).Value = res2Desde;
                oCmd.Parameters.Add("@res2Hasta", SqlDbType.Money).Value = res2Hasta;
                oCmd.Parameters.Add("@tipoRes", SqlDbType.VarChar, 10).Value = tipoResultado;
                oCmd.Parameters.Add("@validador", SqlDbType.VarChar, 10).Value = validador;

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsFallos, "Fallos");                

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsFallos;

        }
    }
}
