using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using System.Data.SqlClient;

namespace Telectronica.Validacion
{
    public class CancelacionVentaDt
    {        
        /// ***********************************************************************************************
        /// <summary>
        /// lista de anomalias de tipo Cancelacion Venta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="codAnomalia">int - Codigo de la anomalia</param>
        /// <param name="parte">int - parte</param>
        /// ***********************************************************************************************
        public static DataSet getAnomalias(Conexion oConn, int codAnomalia, ParteValidacion parte)
        {
            DataSet dsAnomalias = new DataSet();
            dsAnomalias.DataSetName = "CancelacionVenta_AnomaliasDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_CancelacionVenta_getAnomalias";
                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = parte.Estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsAnomalias, "Anomalias");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsAnomalias;
        }
    }
}
