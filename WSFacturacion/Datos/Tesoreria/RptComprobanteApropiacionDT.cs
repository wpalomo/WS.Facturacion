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
    /// Clase que trae los datos para Reportes de Detalle de Apropiacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptComprobanteApropiacionDT
    {


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de Apropiacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="apr">int? - Numero de Apropriacion</param>
        /// ***********************************************************************************************
        public static DataSet getBolsas(Conexion oConn, int estacion,
                int apr)
        {
            DataSet dsApr = new DataSet();
            dsApr.DataSetName = "RptApropiacionDS";

            try
            {

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

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public static void getDetalle(Conexion oConn,DataSet dsApr, int estacion,
                int apr)
        {
            //DataSet dsApr = new DataSet();
            //dsApr.DataSetName = "RptApropiacion_DetallesDS";

            try
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
            catch (Exception ex)
            {
                throw ex;
            }
            return;
            //return dsApr;
        }


    }
}
