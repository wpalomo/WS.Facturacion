using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{
    public class RptOSAsDt
    {

        public static DataSet getTransitosRechazados(Conexion oConn, DateTime desde, DateTime hasta, string fecha, int? coest, int? iAdmTag)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptPeaje_TransitosRechazadosDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptOSAs_getTransitosRechazados";
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = coest;
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@fecha", SqlDbType.Char).Value = fecha;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = iAdmTag;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "ListaTransitos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        public static DataSet getControlTotales(Conexion oConn, DateTime desde, DateTime hasta, int? coest)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_ControlTotalesDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptOSAs_getControlTotales";
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = coest;
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "ControlTotales");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResultado;
        }

        public static DataSet getControlSecuencias(Conexion oConn, DateTime desde, DateTime hasta, string fecha, int? coest, char? estado, int? iAdmTag)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_ControlSecuenciasDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptOSAs_getControlSecuencias";                
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@fecha", SqlDbType.Char).Value = fecha;
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = coest;
                oCmd.Parameters.Add("@estado", SqlDbType.Char).Value = estado;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = iAdmTag;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "ControlSecuencias");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResultado;
        }

        public static DataSet getProximosPagos(Conexion oConn, DateTime desde, DateTime hasta, string fecha, int? iAdmTag)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_ProximosPagosDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptOSAs_getProximosPagos";
                oCmd.Parameters.Add("@fecha", SqlDbType.Char).Value = fecha;
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = iAdmTag;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "ProximosPagos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResultado;
        }
    }
}
