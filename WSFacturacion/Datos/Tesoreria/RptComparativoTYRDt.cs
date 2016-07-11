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
    /// Clase que trae los datos para Reportes de Detalle de Recaudacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptComparativoTYRDt
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de los traficos por jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="jornada">DateTime? - Jornada</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="turno">int? - Turno</param>
        /// ***********************************************************************************************
        public static DataSet getTraficosYRecaudo(Conexion oConn, DateTime? jornada, int? zona, int? estacion, int? turno)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptRecaudacion_Comparativa";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Comparativo";
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = turno;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsPartes, "Traficos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsPartes;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de los carriles solicitados
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="jornada">DateTime? - Jornada</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="Abiertos">bool? - Abiertos</param>
        /// ***********************************************************************************************
        public static DataSet getCantCarrilesXEst(Conexion oConn, DateTime? jornada, int? zona, int? estacion, bool Abiertos)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptRecaudacion_Comparativa";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_EstadoVias";
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@Abiertas", SqlDbType.TinyInt).Value = (Abiertos?1:0);

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsPartes, "Carriles");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de las tarifas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornada">DateTime? - Jornada</param>
        /// ***********************************************************************************************
        public static void getTarifas(Conexion oConn, DataSet dsPartes, int? estacion, DateTime? jornada)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Tarifas";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = jornada;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsPartes, "Tarifas");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }
    }
}
