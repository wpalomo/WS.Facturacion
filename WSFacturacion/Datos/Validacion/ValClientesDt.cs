using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using System.Data.SqlClient;

namespace Telectronica.Validacion
{
    public class ValClientesDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista con los ultimos transitos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        

        /// ***********************************************************************************************
        public static DataSet getUltimosTransitos(Conexion oConn, string patente, DateTime fecha, int top)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "Cliente_TransitosDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Cliente_getUltimosTransitosVehiculo";
                oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = patente;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                oCmd.Parameters.Add("@top", SqlDbType.Int).Value = top;

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "Transitos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

                return dsTransitos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
