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
using Microsoft.SqlServer.Server;

namespace Telectronica.Tesoreria
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Detalle de Apropiacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptComprobantesDT
    {
        #region APROPIACIÓN

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de Apropiacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="apr">int? - Numero de Apropriacion</param>
        /// ***********************************************************************************************
        public static DataSet getBolsas(Conexion oConn, int estacion, int apr)
        {
            DataSet dsApr = new DataSet();
            dsApr.DataSetName = "RptApropiacionDS";

            // cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Apropiacion_getBolsas";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@apr", SqlDbType.Int).Value = apr;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsApr, "Bolsas");

            // Lo cerramos 
            oCmd = null;
            oDA.Dispose();

            return dsApr;
        }
 
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de Apropiacion detalle por denominacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="apr">int? - Numero de Apropriacion</param>
        /// ***********************************************************************************************
        public static void getDetalle(Conexion oConn,DataSet dsApr, int estacion, int apr)
        {
            // Creamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Apropiacion_getDetalle";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@apr", SqlDbType.Int).Value = apr;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsApr, "Detalles");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        #endregion

        #region LIQUIDACIÓN

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de Apropiacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="iEstacion">int? - Codigo de Estacion</param>
        /// <param name="iParte">int? - Numero de Apropriacion</param>
        /// ***********************************************************************************************
        public static DataSet getLiquidacion(Conexion oConn, int iEstacion, int iParte)
        {
            DataSet dsApr = new DataSet();
            dsApr.DataSetName = "RptLiquidacionDS";

            // cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getLiquidacion";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = iEstacion;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = iParte;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsApr, "Liquidacion");

            // Lo cerramos 
            oCmd = null;
            oDA.Dispose();

            return dsApr;
        }

        #endregion

        #region REPOSICIÓN

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Pagos de reposiciones
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="estacion"></param>
        /// <param name="sEstado"></param>
        /// <param name="dtFechaDesde"></param>
        /// <param name="dtFechaHasta"></param>
        /// <param name="iMalote"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getReposicionesPedidas(Conexion oConn, ReposicionPedidaL reposicionesPagadas)
        {
            DataSet dsEntregasFC = new DataSet();
            dsEntregasFC.DataSetName = "RptPagoDeReposicionesDS";
            List<SqlDataRecord> reposiciones = new List<SqlDataRecord>();

            SqlMetaData[] tvp_definition = { new SqlMetaData("reposicion", SqlDbType.Int), new SqlMetaData("coest", SqlDbType.Int) };

            foreach (var item in reposicionesPagadas)
            {
                SqlDataRecord rec = new SqlDataRecord(tvp_definition);
                rec.SetInt32(0, item.Identity);
                rec.SetInt32(1, item.Estacion.Numero);
                reposiciones.Add(rec);
            }

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptMovimientoCajaReposicion_getMovimientoCajaReposicion";

            oCmd.Parameters.Add("@reposiciones", SqlDbType.Structured);
            oCmd.Parameters["@reposiciones"].Direction = ParameterDirection.Input;
            oCmd.Parameters["@reposiciones"].TypeName = "listaReposiciones";
            oCmd.Parameters["@reposiciones"].Value = reposiciones;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsEntregasFC, "PagoDeReposiciones");

            // Cerramos el objeto
            oCmd = null;
            return dsEntregasFC;
        }

        #endregion
    }
}