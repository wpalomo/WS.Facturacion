using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using System.Data.SqlClient;

namespace Telectronica.Validacion
{
    public class RptTransitosDT
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de Transitos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>        
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="parte">int? - Parte</param>  
        /// <param name="via">int? - Via</param>  
        /// ***********************************************************************************************
        public static DataSet getTransitos(Conexion oConn, DateTime jornadaDesde, DateTime jornadaHasta, int? estacion,
            int? parte, int? via)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptValidacion_TransitosDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_RptValidacion_getTransitos";
                oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@via", SqlDbType.TinyInt).Value = via;

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "Transitos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }
    }
}
